using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.Blob
{
    /// <summary>
    /// DTO to utilise the get files function
    /// </summary>
    public class GetFilesDto
    {
        /// <summary>
        /// Prefix from where you want all the files to be listed
        /// </summary>
        public string Prefix { get; set; }
    }
}
