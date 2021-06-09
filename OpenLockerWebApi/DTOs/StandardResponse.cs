using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs
{
    /// <summary>
    /// Standard Response for any response that is given out as a Json
    /// </summary>
    public class StandardResponse
    {
        /// <summary>
        /// Reference code for errors
        /// </summary>
        public int? ErrorCode { get; set; }
        /// <summary>
        /// Actual Response Data for any http request
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// Whether the called action was a success or not
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Message related to the method that was called if any
        /// </summary>
        public string? Message { get; set; }
    }
}
