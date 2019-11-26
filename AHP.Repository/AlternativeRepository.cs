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
    class AlternativeRepository : IAlternativeRepository
    {
        private AHPEntities _context;
        IMapper _mapper;
        public AlternativeRepository(AHPEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IAlternativeModel Add(IAlternativeModel alternative)
        {

            Guid altID = alternative.AlternativeID;
            var allCriteria = _context.Criteria.Where(c => c.ChoiceID == alternative.ChoiceID).ToList();
            var prevAlternatives = _context.Alternatives.Where(c => c.ChoiceID == alternative.ChoiceID).OrderByDescending(x => x.DateCreated).ToArray();
            List<IAlternativeComparisonModel> acs = new List<IAlternativeComparisonModel>();
            int n = prevAlternatives.Length;
            foreach (var item in allCriteria)
            {
                Guid critID = item.CriteriaID;
                for (int i = 0; i < n; i++)
                {
                    IAlternativeComparisonModel a = new AlternativeComparisonModel { CriteriaID = critID, AlternativeID1 = altID, AlternativeID2 = prevAlternatives[i].AlternativeID, DateCreated = DateTime.Now, DateUpdated = DateTime.Now, AlternativeRatio = 0 };
                    acs.Add(a);
                }
            }
            _context.AlternativeComparisons.AddRange(_mapper.Map<List<IAlternativeComparisonModel>, List<AlternativeComparison>>(acs));
            _context.Alternatives.Add(_mapper.Map<IAlternativeModel, Alternative>(alternative));
            return alternative;
        }

        public async Task<IAlternativeModel> GetByIDAsync(params Guid[] idValues)
        {
            var alternative = await _context.Alternatives.FindAsync(idValues[0]);
            return _mapper.Map<Alternative, IAlternativeModel>(alternative);
        }

        public async Task<IAlternativeModel> UpdateAsync(IAlternativeModel alternative)
        {
            var _alternative = await _context.Alternatives.FindAsync(alternative.AlternativeID);
            _context.Entry(_alternative).CurrentValues.SetValues(_mapper.Map<IAlternativeModel, Alternative>(alternative));
            return alternative;
        }

        public async Task<bool> DeleteAsync(IAlternativeModel alternative)
        {
            var _alternative = await _context.Alternatives.FindAsync(alternative.AlternativeID);
            _context.Alternatives.Remove(_alternative);
            return true;
        }


        public async Task<List<IAlternativeModel>> GetPageByChoiceIDAsync(Guid choiceID, int PageNumber, int PageSize = 5)
        {
            List<Alternative> alternatives = new List<Alternative>();
            var altCheck = _context.Alternatives.Where(c => c.ChoiceID == choiceID).FirstOrDefault();
            if (altCheck != null)
            {
                if (_context.Alternatives.Where(c => c.ChoiceID == choiceID).FirstOrDefault().AlternativeScore != null)
                {
                    alternatives = await _context.Alternatives.Where(c => c.ChoiceID == choiceID).OrderByDescending(x => x.AlternativeScore).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

                }
                else
                {
                    alternatives = await _context.Alternatives.Where(c => c.ChoiceID == choiceID).OrderByDescending(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
                }
            }
            else
            {
                alternatives = await _context.Alternatives.Where(c => c.ChoiceID == choiceID).OrderByDescending(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
            }
            return _mapper.Map<List<Alternative>, List<IAlternativeModel>>(alternatives);
        }

        public async Task<List<IAlternativeModel>> GetByChoiceIDAsync(Guid choiceID)
        {
            var alternatives = await _context.Alternatives.Where(c => c.ChoiceID == choiceID).ToListAsync();
            return _mapper.Map<List<Alternative>, List<IAlternativeModel>>(alternatives);
        }

        public List<IAlternativeModel> AddRange(List<IAlternativeModel> alternatives)
        {
            var _alternatives = _mapper.Map<List<IAlternativeModel>, List<Alternative>>(alternatives);
            _context.Alternatives.AddRange(_alternatives);
            return alternatives;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
