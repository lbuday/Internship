using AHP.DAL;
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
    public class CriteriaComparisonRepository : ICriteriaComparisonRepository
    {
        private AHPEntities _context;
        IMapper _mapper;
        public CriteriaComparisonRepository(AHPEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICriteriaComparisonModel Add(ICriteriaComparisonModel cc)
        {

            _context.CriteriaComparisons.Add(_mapper.Map<ICriteriaComparisonModel, CriteriaComparison>(cc));
            return cc;
        }

        public async Task<ICriteriaComparisonModel> GetByIDAsync(Guid CriteriaID1, Guid CriteriaID2)
        {
            var cc = await _context.CriteriaComparisons.FindAsync(CriteriaID1, CriteriaID2);
            return _mapper.Map<CriteriaComparison, ICriteriaComparisonModel>(cc);
        }

        public async Task<ICriteriaComparisonModel> UpdateAsync(ICriteriaComparisonModel cc)
        {
            var _cc = await _context.CriteriaComparisons.FindAsync(cc.CriteriaID1,cc.CriteriaID2);
            _context.Entry(_cc).CurrentValues.SetValues(_mapper.Map<ICriteriaComparisonModel, CriteriaComparison>(cc));
            return cc;
        }

        public async Task<bool> DeleteAsync(ICriteriaComparisonModel cc)
        {
            var _cc = await _context.CriteriaComparisons.FindAsync(cc.CriteriaID1, cc.CriteriaID2);
            _context.CriteriaComparisons.Remove(_cc);
            return true;
        }

        public async Task<bool> DeleteByCriteriaIDAsync(Guid criteriaID)
        {
            var ccs = await _context.CriteriaComparisons.Where(cc => cc.CriteriaID1 == criteriaID || cc.CriteriaID2 == criteriaID).ToListAsync();
            _context.CriteriaComparisons.RemoveRange(ccs);
            return true;
        }

        public async Task<List<ICriteriaComparisonModel>> GetPageByCriterionIDAsync(Guid criteriaID,  int PageNumber, int PageSize = 10)
        {
            var ccs = await _context.CriteriaComparisons.Where(cc => (cc.CriteriaID1 == criteriaID || cc.CriteriaID2 == criteriaID) && (cc.CriteriaRatio != 0)).OrderBy(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
            return _mapper.Map<List<CriteriaComparison>, List<ICriteriaComparisonModel>>(ccs);
        }

        public async Task<List<ICriteriaComparisonModel>> GetByFirstCriterionIDAsync(Guid criteriaID)
        {
            var ccs = await _context.CriteriaComparisons.Where(cc => cc.CriteriaID1 == criteriaID).OrderBy(x => x.DateCreated).ToListAsync();
            return _mapper.Map<List<CriteriaComparison>, List<ICriteriaComparisonModel>>(ccs);
        }

        public List<ICriteriaComparisonModel> AddRange(List<ICriteriaComparisonModel> ccs)
        {
            var _ccs = _mapper.Map<List<ICriteriaComparisonModel>, List<CriteriaComparison>>(ccs);
            _context.CriteriaComparisons.AddRange(_ccs);
            return ccs;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ICriteriaComparisonModel>> GetUnfilledAsync(Guid choiceID, int PageSize = 10)
        {
            var criteriaFromChoice = await _context.Criteria.Where(c => c.ChoiceID == choiceID).ToListAsync();
            List<Guid> criteriaIDs = new List<Guid>();
            foreach (var item in criteriaFromChoice)
            {
                criteriaIDs.Add(item.CriteriaID);
            }
            var ccs = await _context.CriteriaComparisons.Where(cc => criteriaIDs.Contains(cc.CriteriaID1) && cc.CriteriaRatio == 0).OrderBy(x => x.DateCreated).Take(PageSize).ToListAsync();
            return _mapper.Map<List<CriteriaComparison>, List<ICriteriaComparisonModel>>(ccs);
        }
    }
}
