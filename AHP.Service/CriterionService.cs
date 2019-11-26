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
    class CriterionService : ICriterionService
    {

        ICriterionRepository _critRepo;
        IUnitOfWorkFactory _unitFactory;

        public CriterionService(ICriterionRepository critRepo, IUnitOfWorkFactory unitFactory)
        {
            _critRepo = critRepo;
            _unitFactory = unitFactory;
        }
        public async Task<ICriterionModel> AddAsync(ICriterionModel criterion)
        {
            criterion.CriteriaID = Guid.NewGuid();
            criterion.DateCreated = DateTime.Now;
            criterion.DateUpdated = DateTime.Now;
            criterion.CriteriaScore = null;

            using (var uof = _unitFactory.Create())
            {
                criterion = _critRepo.Add(criterion);
                await _critRepo.SaveAsync();
                uof.Commit();
            }
            return criterion;
        }
        public async Task<List<ICriterionModel>> GetAsync(Guid id, int page =1)
        {
            var criteria = await _critRepo.GetPageByChoiceIDAsync(id, page);
            return criteria;
        }
        public async Task<bool> DeleteAsync(ICriterionModel criteria)
        {
            using (var uof = _unitFactory.Create())
            {
                var deleted = await _critRepo.DeleteAsync(criteria);
                await _critRepo.SaveAsync();
                uof.Commit();
                return deleted;
            }
        }
        public async Task<bool> UpdateAsync(ICriterionModel criteria)
        {
           
                var updated = await _critRepo.UpdateAsync(criteria);
                await _critRepo.SaveAsync();
              
                return true;
        }

        public async Task<ICriterionModel> GetByIdAsync(Guid criterionID)
        {
            var criteria = await _critRepo.GetByIDAsync(criterionID);
            return criteria;
        }
    }
}
