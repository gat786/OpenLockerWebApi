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

        public void CreateUser(User user)
        {
            _userCollection.InsertOne(user);
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userCollection.Find("{}").Limit(10).ToCursor().ToEnumerable();
        }

        public void GetUserByEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public void GetUserByUsername(string userName)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
