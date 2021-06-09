using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Models
{
    public class OpenLockerSettings: IOpenLockerSettings
    {
        public string DatabaseName { get; set; }
        public string MongoDbConnectionString { get; set; }
        public string JwtSigningKey { get; set; }
        public string BlobStorageConnectionString { get; set; }
    }

    public interface IOpenLockerSettings
    {
        public string DatabaseName { get; set; }
        public string MongoDbConnectionString { get; set; }
        public string JwtSigningKey { get; set; }
        public string BlobStorageConnectionString { get; set; }
    }
}
