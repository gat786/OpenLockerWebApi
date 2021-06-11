using MongoDB.Driver;
using OpenLockerWebApi.Models;
using OpenLockerWebApi.Services.UserService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Services.UserService
{
    public class MongoUserService : IUserService
    {
        
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _userCollection;

        public MongoUserService(IMongoDatabase database)
        {
            _database = database;
            _userCollection = _database.GetCollection<User>("users");
        }

        public bool AddRefreshToken(User user, RefreshToken refreshToken)
        {
            var updatedBuilder = Builders<User>.Update.Set<RefreshToken>(nameof(User.RefreshToken), refreshToken);
            _userCollection.UpdateOneAsync(userItem => userItem.Username == user.Username, updatedBuilder);
            return true;
        }

        public bool RemoveRefreshToken(User user, RefreshToken tokenToRemove)
        {
            var updatedBuilder = Builders<User>.Update.Set<RefreshToken>(nameof(User.RefreshToken), tokenToRemove);
            _userCollection.UpdateOneAsync(userItem => userItem.Username == user.Username, updatedBuilder);
            return true;
        }

        public User CreateUser(User user)
        {
            _userCollection.InsertOne(user);
            return user;
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userCollection.Find("{}").Limit(10).ToCursor().ToEnumerable();
        }

        public User GetByEmailOrUsername(string emailAddress, string username)
        {
            return _userCollection.Find<User>(user => user.Username == username || user.EmailAddress == emailAddress).FirstOrDefault();
        }

        public User GetUserByEmail(string emailAddress)
        {
            return _userCollection.Find<User>(user => user.EmailAddress == emailAddress).FirstOrDefault();
        }

        public User GetUserByUsername(string userName)
        {
            return _userCollection.Find<User>(user => user.Username == userName).FirstOrDefault();
        }

        

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUserFromRefreshToken(string token)
        {
            var user = _userCollection.Find(user => user.RefreshToken.Token == token).FirstOrDefault();
            return user;
        }
    }
}
