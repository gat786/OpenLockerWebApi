using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.Blob
{
    /// <summary>
    /// DTO to be received from user when a delete operation is to be performed
    /// </summary>
    public class DeleteBlobDto
    {
        /// <summary>
        /// File name which has to be deleted. Prefixes should be included in the filename
        /// </summary>
        [Required]
        public string FileName { get; set; }
    }
}
