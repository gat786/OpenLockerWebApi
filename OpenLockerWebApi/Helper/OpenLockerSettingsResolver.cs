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
                ConnectionString = Environment.GetEnvironmentVariable(nameof(OpenLockerSettings.ConnectionString))
            };
            return openLockerSettings;
        }
    }
}
