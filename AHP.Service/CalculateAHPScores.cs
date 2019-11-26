using AHP.Model.Common;
using AHP.Repository.Common;
using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    class CalculateAHPScores : ICalculateAHPScores
    {
        IUnitOfWorkFactory _unitOfWorkFactory;
        ICriterionRepository _criterionRepository;
        IAlternativeRepository _alternativeRepository;
        IAlternativeComparisonRepository _alternativeComparisonRepo;
        ICriteriaComparisonRepository _criteriaComparisonRepo;
        IAHPService _AHPService;

        public CalculateAHPScores(
            IUnitOfWorkFactory unitOfWorkFactory,
            IAlternativeRepository alternativeRepository,
            IAlternativeComparisonRepository alternativeComparisonRepo,
            ICriterionRepository criterionRepository,
            ICriteriaComparisonRepository criteriaComparisonRepo,
            IAHPService AHPService)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _alternativeRepository = alternativeRepository;
            _alternativeComparisonRepo = alternativeComparisonRepo;
            _criterionRepository = criterionRepository;
            _criteriaComparisonRepo = criteriaComparisonRepo;
            _AHPService = AHPService;
        }
        private List<string> inconsistencies = new List<string>();
        public async Task<bool> CalculateCriteriaWeights(Guid choiceId)
        {
            var criteria = await _criterionRepository.GetByChoiceIDAsync(choiceId);
            var hash = new Dictionary<Guid, int>();
            int i = 0;
            foreach (var criterion in criteria)
            {
                hash[criterion.CriteriaID] = i;
                i++;
            }
            double[,] matrix = new double[criteria.Count, criteria.Count];
            foreach (var criterion in criteria)
            {
                matrix[hash[criterion.CriteriaID], hash[criterion.CriteriaID]] = 1;
                var comparisons = await _criteriaComparisonRepo.GetByFirstCriterionIDAsync(criterion.CriteriaID);
                foreach (var comparison in comparisons)
                {
                    matrix[hash[comparison.CriteriaID1], hash[comparison.CriteriaID2]] = comparison.CriteriaRatio;
                    matrix[hash[comparison.CriteriaID2], hash[comparison.CriteriaID1]] = 1 / comparison.CriteriaRatio;
                }
            }
            var weights = _AHPService.CalculatePriortyVector(matrix);

            double consistency = _AHPService.CalculateConsistency(matrix, weights);
            if (consistency > 0.1)
            {
                //using (var uof = _unitOfWorkFactory.Create())
                //{

                    foreach (var criterion in criteria)
                    {
                        var comparisons = await _criteriaComparisonRepo.GetByFirstCriterionIDAsync(criterion.CriteriaID);
                        foreach (var comparison in comparisons)
                        {
                            comparison.CriteriaRatio = 0;
                            await _criteriaComparisonRepo.UpdateAsync(comparison);
                        }
                    }
                    await _criteriaComparisonRepo.SaveAsync();
                    //uof.Commit();
                    return false;

               // }

            }
            return await CalculateAlternativeWeights(choiceId, weights, criteria);

        }


        public async Task<bool> CalculateAlternativeWeights(Guid choiceId, double[] choiceWeights, List<ICriterionModel> criteria)
        {
            var alternatives = await _alternativeRepository.GetByChoiceIDAsync(choiceId);
            var hash = new Dictionary<Guid, int>();
            int i = 0;
            foreach (var alternative in alternatives)
            {
                hash[alternative.AlternativeID] = i;
                i++;
            }
            var weights = new List<double[]>();
            foreach (var criterion in criteria)
            {
                double[,] matrix = new double[alternatives.Count, alternatives.Count];
                foreach (var alternative in alternatives)
                {
                    matrix[hash[alternative.AlternativeID], hash[alternative.AlternativeID]] = 1;
                    var comparisons = await _alternativeComparisonRepo.GetByCriteriaAndFirstAlternativeIDAsync(criterion.CriteriaID, alternative.AlternativeID);
                    foreach (var comparison in comparisons)
                    {
                        matrix[hash[comparison.AlternativeID1], hash[comparison.AlternativeID2]] = comparison.AlternativeRatio;
                        matrix[hash[comparison.AlternativeID2], hash[comparison.AlternativeID1]] = 1 / comparison.AlternativeRatio;
                    }
                }
                var weight = _AHPService.CalculatePriortyVector(matrix);
                var consistency = _AHPService.CalculateConsistency(matrix, weight);
                if (consistency > 0.1)
                {


                    foreach (var alternative in alternatives)
                    {
                        var comparisons = await _alternativeComparisonRepo.GetByCriteriaAndFirstAlternativeIDAsync(criterion.CriteriaID, alternative.AlternativeID);
                        foreach (var comparison in comparisons)
                        {
                            comparison.AlternativeRatio = 0;
                            await _alternativeComparisonRepo.UpdateAsync(comparison);
                        }
                    }
                    await _alternativeComparisonRepo.SaveAsync();
                    return false;


                }
                weights.Add(weight);
            }
            double[,] alternativeWeightMatrix = new double[alternatives.Count, criteria.Count];
            i = 0;
            foreach (var weight in weights)
            {
                for (int j = 0; j < alternatives.Count; j++)
                {
                    alternativeWeightMatrix[j, i] = weight[j];
                }
                i++;
            }
            var alternativeScores = _AHPService.FinalCalculate(choiceWeights, alternativeWeightMatrix);
            //sprema dobivene scoreove

            using (var uof = _unitOfWorkFactory.Create())
            {
                i = 0;
                foreach (var alternative in alternatives)
                {
                    alternative.AlternativeScore = alternativeScores[i];
                    await _alternativeRepository.UpdateAsync(alternative);
                    i++;
                }
                await _alternativeRepository.SaveAsync();
                uof.Commit();
            }
            return true;

        }

    }
}
