using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository.Common
{
    public interface ICriterionRepository : IRepository<ICriterionModel>
    {
        Task<List<ICriterionModel>> GetPageByChoiceIDAsync(Guid choiceID, int pageNumber, int pageSize = 5);
        Task<ICriterionModel> LoadCriteriaComparisonsPageAsync(ICriterionModel criterion, int PageNumber, int PageSize = 5);
        Task<List<ICriterionModel>> GetByChoiceIDAsync(Guid choiceID);
        Task<ICriterionModel> GetByIDAsync(params Guid[] idValues);
    }
}
