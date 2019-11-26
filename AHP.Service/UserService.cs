using AHP.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AHP.Service.Common;
using AHP.Model.Common;

namespace AHP.Service.UserCRUD
{
    class UserService : IUserService
    {

        IUnitOfWorkFactory _unitOfWorkFactory;
        IUserRepository _userRepository;
        public UserService(IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userRepository = userRepository;
        }
        public async Task<IUserModel> CheckAsync(IUserModel user)
        {
            IUserModel _user = await _userRepository.GetByUsernameAsync(user.Username);
            if (_user != null)
            {
                return null;
            }
            else
            {
                user.UserID = Guid.NewGuid();
                user.DateCreated = DateTime.Now;
                user.DateUpdated = DateTime.Now;
              
                    user = _userRepository.Add(user);
                    await _userRepository.SaveAsync();
              
                return user;
            }
        }

        public async Task<IUserModel> GetAsync(IUserModel user)
        {
            var _user = await _userRepository.GetByUsernameAsync(user.Username);
            if (_user != null)
            {
                if (_user.Password == user.Password)
                {
                    return _user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<IUserModel> UpdateAsync(IUserModel user)
        {

            IUserModel updated;
            var _baseUser = await _userRepository.GetByIDAsync(user.UserID);
      
                _baseUser.DateUpdated = DateTime.Now;
                if (user.Password != null) _baseUser.Password = user.Password;
                if (user.Username != null) _baseUser.Username = user.Username;
                updated = await _userRepository.UpdateAsync(_baseUser);
                await _userRepository.SaveAsync();
              
            return updated;
        }
        public async Task<bool> DeleteAsync(IUserModel user)
        {
            bool a = true;
            
                a = await _userRepository.DeleteAsync(user);
                await _userRepository.SaveAsync();
             
            return a;
        }

    }
}
