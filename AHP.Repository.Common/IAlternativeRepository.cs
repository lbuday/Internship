using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository.Common
{
    public interface IAlternativeRepository : IRepository<IAlternativeModel>
    {
        Task<List<IAlternativeModel>> GetPageByChoiceIDAsync(Guid choiceID, int PageNumber, int PageSize = 5 );
        Task<List<IAlternativeModel>> GetByChoiceIDAsync(Guid choiceID);
        Task<IAlternativeModel> GetByIDAsync(params Guid[] idValues);
    }
}
