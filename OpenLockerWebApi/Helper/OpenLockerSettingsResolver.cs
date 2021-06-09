using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Helper
{
    /// <summary>
    /// Class to store DatabaseSettings and load it from .env files
    /// </summary>
    public static class OpenLockerSettingsResolver
    {
        public static OpenLockerSettings FromEnvironment()
        {
            var openLockerSettings = new OpenLockerSettings
            {
                DatabaseName = Environment.GetEnvironmentVariable(nameof(OpenLockerSettings.DatabaseName)),
                MongoDbConnectionString = Environment.GetEnvironmentVariable(nameof(OpenLockerSettings.MongoDbConnectionString)),
                JwtSigningKey = Environment.GetEnvironmentVariable(nameof(OpenLockerSettings.JwtSigningKey)),
                BlobStorageConnectionString = Environment.GetEnvironmentVariable(nameof(OpenLockerSettings.BlobStorageConnectionString)),
            };
            return openLockerSettings;
        }
    }
}
