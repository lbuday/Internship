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
    class AlternativeComparisonService : IAlternativeComparisonService
    {
        IAlternativeComparisonRepository _altCompRepo;
        IUnitOfWorkFactory _unitFactory;
        public AlternativeComparisonService(
            IAlternativeComparisonRepository altCompRepo,
            IUnitOfWorkFactory unitOfWorkFactory
            )
        {
            _altCompRepo = altCompRepo;
            _unitFactory = unitOfWorkFactory;
        }

        public async Task<List<IAlternativeComparisonModel>> AddAsync(List<IAlternativeComparisonModel> comparisons)
        {
          
                foreach (IAlternativeComparisonModel comparison in comparisons)
                {
                    comparison.DateCreated = DateTime.Now;
                    comparison.DateUpdated = DateTime.Now;
                    comparison.AlternativeRatio = 0;
                }
                comparisons = _altCompRepo.AddRange(comparisons);
                await _altCompRepo.SaveAsync();
               
            

            return comparisons;

        }
        public async Task<List<IAlternativeComparisonModel>> GetAsync(Guid alternativeId, Guid criteriaId, int page = 1)
        {
            var alternatives = await _altCompRepo.GetByCriteriaAlternativesIDAsync(criteriaId, alternativeId, page);

            return alternatives;
        }

        public async Task<List<IAlternativeComparisonModel>> GetByAlternativeIdAsync(Guid alternativeId, int page = 1)
        {
            var alternatives = await _altCompRepo.GetByAlternativesIDAsync(alternativeId, page);

            return alternatives;
        }

        public async Task<List<IAlternativeComparisonModel>> GetUnfilledAsync(Guid ChoiceID)
        {
            var comparisons = await _altCompRepo.GetUnfilledAsync(ChoiceID);

            return comparisons;
        }

        public async Task<List<IAlternativeComparisonModel>> UpdateAsync(List<IAlternativeComparisonModel> comparisons)
        {
            using (var uof = _unitFactory.Create())
            {

                foreach (IAlternativeComparisonModel comparison in comparisons)
                {
                    var baseComparison = await _altCompRepo.GetByIDAsync(comparison.CriteriaID, comparison.AlternativeID1, comparison.AlternativeID2 );
                    baseComparison.DateUpdated = DateTime.Now;
                    baseComparison.AlternativeRatio = comparison.AlternativeRatio;
                    await _altCompRepo.UpdateAsync(baseComparison);
                }
                await _altCompRepo.SaveAsync();
                uof.Commit();
                return comparisons;
            }
        }

        
    }
}
