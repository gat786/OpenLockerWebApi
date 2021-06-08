using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Models
{
    public class OpenLockerSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
