using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.Blob
{
    /// <summary>
    /// File Details which is to be downloaded
    /// </summary>
    public class DownloadFileDto
    {
        /// <summary>
        /// name of the file prefixes included from the root of the container
        /// </summary>
        [Required]
        public string FileName { get; set; }
    }
}
