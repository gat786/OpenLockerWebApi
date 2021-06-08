using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Services.UserService
{
    public class MockUserService : IUserService
    {
        public User CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetByEmailOrUsername(string emailAddress, string username)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string userName)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
