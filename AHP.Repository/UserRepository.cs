using AHP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AHP.Repository.Common;
using System.Data.Entity;
using AHP.Model;
using AutoMapper;
using AHP.Model.Common;

namespace AHP.Repository
{

    public class UserRepository : IUserRepository

    {
        private AHPEntities _context;
        IMapper _mapper;
        public UserRepository(AHPEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IUserModel Add(IUserModel user)
        {

            _context.Users.Add(_mapper.Map<IUserModel, User>(user));
            return user;
        }

        public async Task<IUserModel> GetByIDAsync(params Guid[] idValues)
        {
            var user = await _context.Users.FindAsync(idValues[0]);
            return _mapper.Map<User, IUserModel>(user);
        }

        public async Task<IUserModel> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            return _mapper.Map<User, IUserModel>(user);
        }

        public async Task<IUserModel> UpdateAsync(IUserModel user)
        {
            var _user = await _context.Users.FindAsync(user.UserID);
            _context.Entry(_user).CurrentValues.SetValues(_mapper.Map<IUserModel, User>(user));
            return user; 
        }

        public async Task<bool> DeleteAsync(IUserModel user)
        {
            var _user = await _context.Users.FindAsync(user.UserID);
            _context.Users.Remove(_user);
            return true;
        }


        public async Task<IUserModel> LoadChoicesPageAsync(IUserModel user, int PageNumber , int PageSize = 5)
        {
            var _user = await _context.Users.FindAsync(user.UserID);
            await _context.Entry(_user).Collection(u => u.Choices).Query().OrderBy(x => x.DateCreated).Skip((PageNumber - 1) * PageSize).Take(PageSize).LoadAsync();
           return _mapper.Map<User, IUserModel>(_user); ;
        }

        public List<IUserModel> AddRange(List<IUserModel> users)
        {
            var _users = _mapper.Map<List<IUserModel>, List<User>>(users);
            _context.Users.AddRange(_users);
            return users;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
