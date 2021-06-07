using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Models
{
    public class OpenLockerDatabaseSettings : IOpenLockerDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IOpenLockerDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

        static IOpenLockerDatabaseSettings FromSection(IConfigurationSection section)
        {
            var openLockerSettings = new OpenLockerDatabaseSettings
            {
                DatabaseName = section.GetValue<string>(nameof(DatabaseName)),
                ConnectionString = section.GetValue<string>(nameof(ConnectionString))
            };
            return openLockerSettings;
        }
    }
}
