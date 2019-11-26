using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service.Common
{
    public interface IChoiceService
    {
        Task<IChoiceModel> CreateAsync(IChoiceModel choice);
        Task<IChoiceModel> UpdateAsync(IChoiceModel choice);
        Task<List<IChoiceModel>> GetAsync(Guid userId, int page);
        Task<bool> DeleteAsync(IChoiceModel choice);
        Task<IChoiceModel> GetByIdAsync(Guid choiceID);
    }
}
