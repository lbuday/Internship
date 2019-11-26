using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AHP.Model.Common;

namespace AHP.Service.Common
{
    public interface ICriterionService
    {
        Task<ICriterionModel> AddAsync(ICriterionModel criterion);
        Task<bool> DeleteAsync(ICriterionModel criteria);
        Task<List<ICriterionModel>> GetAsync(Guid id, int page = 1);
        Task<bool> UpdateAsync(ICriterionModel criteria);
        Task<ICriterionModel> GetByIdAsync(Guid criterionID);
    }
}