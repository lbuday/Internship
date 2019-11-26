using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service.Common
{
    public interface IUserService
    {
        Task<IUserModel> UpdateAsync(IUserModel user);
        Task<IUserModel> CheckAsync(IUserModel user);
        Task<IUserModel> GetAsync(IUserModel user);
        Task<bool> DeleteAsync(IUserModel user);
    }
}
