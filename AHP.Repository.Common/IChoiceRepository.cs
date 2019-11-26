using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository.Common
{
    public interface IChoiceRepository : IRepository<IChoiceModel>
    {
        Task<List<IChoiceModel>> GetChoicesByUserIDAsync(Guid userID, int PageNumber, int PageSize = 5);
        Task<IChoiceModel> LoadCriteriaPageAsync(IChoiceModel choice, int PageNumber, int PageSize = 5);
        Task<IChoiceModel> LoadAlternativesPageAsync(IChoiceModel choice, int PageNumber, int PageSize = 5);
        Task<IChoiceModel> GetByIDAsync(params Guid[] idValues);
    }
}
