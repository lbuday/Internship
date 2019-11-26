using AHP.Model;
using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository.Common
{
    public interface IAlternativeComparisonRepository : IRepository<IAlternativeComparisonModel>
    {
        Task<List<IAlternativeComparisonModel>> GetByCriteriaAlternativesIDAsync(Guid criteriaID, Guid alternativeID, int PageNumber, int PageSize = 10);
        Task<List<IAlternativeComparisonModel>> GetByAlternativesIDAsync(Guid alternativeID, int PageNumber, int PageSize = 10);
        Task<bool> DeleteByAlternativeIDAsync(Guid alternativeID);
        Task<bool> DeleteByCriteriaIDAsync(Guid criteriaID);
        Task<List<IAlternativeComparisonModel>> GetByCriteriaIDAsync(Guid criteriaID);
        Task<List<IAlternativeComparisonModel>> GetUnfilledAsync(Guid choiceID, int PageSize = 10);
        Task<IAlternativeComparisonModel> GetByIDAsync(Guid CriteriaId, Guid AlternativeID1, Guid AlternativeID2);
        Task<List<IAlternativeComparisonModel>> GetByCriteriaAndFirstAlternativeIDAsync(Guid criteriaID, Guid alternativeID);
    }
}
