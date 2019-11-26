using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository.Common
{
    public interface ICriteriaComparisonRepository : IRepository<ICriteriaComparisonModel>
    {
        Task<List<ICriteriaComparisonModel>> GetPageByCriterionIDAsync(Guid criteriaID, int PageNumber, int PageSize = 10);
        Task<List<ICriteriaComparisonModel>> GetByFirstCriterionIDAsync(Guid criteriaID);
        Task<bool> DeleteByCriteriaIDAsync(Guid criteriaID);
        Task<List<ICriteriaComparisonModel>> GetUnfilledAsync(Guid choiceID, int PageSize = 10);
        Task<ICriteriaComparisonModel> GetByIDAsync(Guid CriteriaID1, Guid CriteriaID2);

    }
}
