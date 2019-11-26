using AHP.DAL;
using AHP.Model;
using AHP.Model.Common;
using AHP.Repository.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository
{
    class CriterionRepository : ICriterionRepository
    {
        private AHPEntities _context;
        private IMapper _mapper;
        public CriterionRepository(AHPEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICriterionModel Add(ICriterionModel criterion)
        {
            Guid critID = criterion.CriteriaID;
            var prevCriteria = _context.Criteria.Where(c => c.ChoiceID == criterion.ChoiceID).ToList();
            List<ICriteriaComparisonModel> ccs = new List<ICriteriaComparisonModel>();
            foreach (var item in prevCriteria)
            {
                ICriteriaComparisonModel c = new CriteriaComparisonModel { CriteriaID1 = critID, CriteriaID2 = item.CriteriaID, DateCreated = DateTime.Now, DateUpdated = DateTime.Now, CriteriaRatio = 0 };
                ccs.Add(c);
            }
            _context.CriteriaComparisons.AddRange(_mapper.Map<List<ICriteriaComparisonModel>, List<CriteriaComparison>>(ccs));
            var prevAlternatives = _context.Alternatives.Where(c => c.ChoiceID == criterion.ChoiceID).OrderByDescending(x => x.DateCreated).ToArray();
            List<IAlternativeComparisonModel> acs = new List<IAlternativeComparisonModel>();
            int n = prevAlternatives.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    IAlternativeComparisonModel a = new AlternativeComparisonModel { CriteriaID = critID, AlternativeID1 = prevAlternatives[i].AlternativeID, AlternativeID2 = prevAlternatives[j].AlternativeID, DateCreated = DateTime.Now, DateUpdated = DateTime.Now, AlternativeRatio = 0 };
                    acs.Add(a);
                }
            }
            _context.AlternativeComparisons.AddRange(_mapper.Map<List<IAlternativeComparisonModel>, List<AlternativeComparison>>(acs));
            _context.Criteria.Add(_mapper.Map<ICriterionModel, Criterion>(criterion));
            return criterion;
        }

        public List<ICriterionModel> AddRange(List<ICriterionModel> criteria)
        {
            _context.Criteria.AddRange(_mapper.Map<List<ICriterionModel>, List<Criterion>>(criteria));
            return criteria;
        }

        public async Task<bool> DeleteAsync(ICriterionModel criterion)
        {
            var _criterion = await _context.Criteria.FindAsync(criterion.CriteriaID);
            _context.Criteria.Remove(_criterion);
            return true;
        }

        public async Task<ICriterionModel> GetByIDAsync(params Guid[] idValues)
        {
            var criterion = await _context.Criteria.FindAsync(idValues[0]);
            return _mapper.Map<Criterion, ICriterionModel>(criterion);
        }


        public async Task<ICriterionModel> UpdateAsync(ICriterionModel criterion)
        {
            var _criterion = await _context.Criteria.FindAsync(criterion.CriteriaID);
            _context.Entry(_criterion).CurrentValues.SetValues(_mapper.Map<ICriterionModel, Criterion>(criterion));
            return criterion;
        }

        public async Task<List<ICriterionModel>> GetPageByChoiceIDAsync(Guid choiceID, int pageNumber, int pageSize = 5)
        {
            var criteria = await _context.Criteria.Where(c => c.ChoiceID == choiceID).OrderByDescending(x => x.DateCreated).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return _mapper.Map<List<Criterion>, List<ICriterionModel>>(criteria);
        }

        public async Task<List<ICriterionModel>> GetByChoiceIDAsync(Guid choiceID)
        {
            var criteria = await _context.Criteria.Where(c => c.ChoiceID == choiceID).ToListAsync();
            return _mapper.Map<List<Criterion>, List<ICriterionModel>>(criteria);
        }

        public async Task<ICriterionModel> LoadCriteriaComparisonsPageAsync(ICriterionModel criterion, int PageNumber, int PageSize = 5)
        {
            var _criterion = await _context.Criteria.FindAsync(criterion.CriteriaID);
            await _context.Entry(_criterion).Collection(c => c.CriteriaComparisons).Query().OrderBy(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).LoadAsync();
            await _context.Entry(_criterion).Collection(c => c.CriteriaComparisons1).Query().OrderBy(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).LoadAsync();
            return _mapper.Map<Criterion, ICriterionModel>(_criterion);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
