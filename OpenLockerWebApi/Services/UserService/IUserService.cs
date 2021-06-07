using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Services.UserService
{
    public interface IUserService
    {
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void GetUserByEmail(string emailAddress);
        void GetUserByUsername(string userName);
        IEnumerable<User> GetAllUsers();
    }
}
