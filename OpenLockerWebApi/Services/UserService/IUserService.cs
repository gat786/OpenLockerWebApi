using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Services.UserService
{
    public interface IUserService
    {
        User CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        User GetUserByEmail(string emailAddress);
        User GetUserByUsername(string userName);
        User GetByEmailOrUsername(string emailAddress, string username);

        bool AddRefreshToken(User user, RefreshToken refreshToken);

        User GetUserFromRefreshToken(string token);

        bool RemoveRefreshToken(User user, RefreshToken tokenToRemove);

        IEnumerable<User> GetAllUsers();
    }
}
