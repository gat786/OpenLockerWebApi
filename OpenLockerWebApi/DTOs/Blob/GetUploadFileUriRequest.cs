using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.Blob
{
    /// <summary>
    /// File Details which is requested to be uploaded
    /// </summary>
    public class GetUploadFileUriRequest
    {
        /// <summary>
        /// Filename including the prefix from the root of container
        /// </summary>
        public string FileName { get; set; }
    }
}
