using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string id { get; set; }
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public override string ToString()
        {
            return $"{Username} {EmailAddress} {Password}";
        }
    }
}
