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
    public class AlternativeComparisonRepository : IAlternativeComparisonRepository
    {
        private AHPEntities _context;
        IMapper _mapper;
        public AlternativeComparisonRepository(AHPEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IAlternativeComparisonModel Add(IAlternativeComparisonModel ac)
        {

            _context.AlternativeComparisons.Add(_mapper.Map<IAlternativeComparisonModel, AlternativeComparison>(ac));
            return ac;
        }

        public async Task<IAlternativeComparisonModel> GetByIDAsync(Guid CriteriaId, Guid AlternativeID1, Guid AlternativeID2)
        {
            var ac = await _context.AlternativeComparisons.FindAsync(CriteriaId, AlternativeID1, AlternativeID2);
            return _mapper.Map<AlternativeComparison, IAlternativeComparisonModel>(ac);
        }

        public async Task<bool> DeleteByAlternativeIDAsync(Guid alternativeID)
        {
            var acs = await _context.AlternativeComparisons.Where(ac => ac.AlternativeID1 == alternativeID || ac.AlternativeID2 == alternativeID).ToListAsync();
            _context.AlternativeComparisons.RemoveRange(acs);
            return true;
        }

        public async Task<bool> DeleteByCriteriaIDAsync(Guid criteriaID)
        {
            var acs = await _context.AlternativeComparisons.Where(ac => ac.CriteriaID == criteriaID).ToListAsync();
            _context.AlternativeComparisons.RemoveRange(acs);
            return true;
        }

        public async Task<IAlternativeComparisonModel> UpdateAsync(IAlternativeComparisonModel ac)
        {
            var _ac = await _context.AlternativeComparisons.FindAsync(ac.CriteriaID,ac.AlternativeID1, ac.AlternativeID2);
            _context.Entry(_ac).CurrentValues.SetValues(_mapper.Map<IAlternativeComparisonModel, AlternativeComparison>(ac));
            return ac;
        }

        public async Task<bool> DeleteAsync(IAlternativeComparisonModel ac)
        {
            var _ac = await _context.AlternativeComparisons.FindAsync(ac.AlternativeID1, ac.AlternativeID2);
            _context.AlternativeComparisons.Remove(_ac);
            return true;
        }


        public async Task<List<IAlternativeComparisonModel>> GetByCriteriaAlternativesIDAsync(Guid criteriaID, Guid alternativeID,  int PageNumber, int PageSize = 10)
        {
            var acs = await _context.AlternativeComparisons.Where(ac => ac.CriteriaID == criteriaID && (ac.AlternativeID1 == alternativeID || ac.AlternativeID2 == alternativeID) && (ac.AlternativeRatio == 0)).OrderBy(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
            return _mapper.Map<List<AlternativeComparison>, List<IAlternativeComparisonModel>>(acs);
        }

        public async Task<List<IAlternativeComparisonModel>> GetByCriteriaIDAsync(Guid criteriaID)
        {
            var acs = await _context.AlternativeComparisons.Where(ac => ac.CriteriaID == criteriaID).ToListAsync();
            return _mapper.Map<List<AlternativeComparison>, List<IAlternativeComparisonModel>>(acs);
        }

        public List<IAlternativeComparisonModel> AddRange(List<IAlternativeComparisonModel> acs)
        {
            var _acs = _mapper.Map<List<IAlternativeComparisonModel>, List<AlternativeComparison>>(acs);
            _context.AlternativeComparisons.AddRange(_acs);
            return acs;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<IAlternativeComparisonModel>> GetUnfilledAsync(Guid choiceID, int PageSize = 10)
        {
            var alternativeFromChoice = await _context.Alternatives.Where(c => c.ChoiceID == choiceID).ToListAsync();
            List<Guid> AlternativeIDs = new List<Guid>();
            foreach (var item in alternativeFromChoice)
            {
                AlternativeIDs.Add(item.AlternativeID);
            }
            var ccs = await _context.AlternativeComparisons.Where(ac => AlternativeIDs.Contains(ac.AlternativeID1) && (ac.AlternativeRatio == 0)).OrderBy(x => x.DateCreated).Take(PageSize).ToListAsync();
            return _mapper.Map<List<AlternativeComparison>, List<IAlternativeComparisonModel>>(ccs);
        }

        public async Task<List<IAlternativeComparisonModel>> GetByAlternativesIDAsync(Guid alternativeID, int PageNumber, int PageSize = 10)
        {
            var acs = await _context.AlternativeComparisons.Where(ac => (ac.AlternativeID1 == alternativeID || ac.AlternativeID2 == alternativeID)  && (ac.AlternativeRatio != 0)).OrderByDescending(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
            return _mapper.Map<List<AlternativeComparison>, List<IAlternativeComparisonModel>>(acs);
        }

        public async Task<List<IAlternativeComparisonModel>> GetByCriteriaAndFirstAlternativeIDAsync(Guid criteriaID, Guid alternativeID)
        {
            var acs = await _context.AlternativeComparisons.Where(ac => ac.CriteriaID == criteriaID && ac.AlternativeID1 == alternativeID).ToListAsync();
            return _mapper.Map<List<AlternativeComparison>, List<IAlternativeComparisonModel>>(acs);
        }
    }
}
