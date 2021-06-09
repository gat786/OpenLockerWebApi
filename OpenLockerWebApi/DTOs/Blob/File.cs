using System;

namespace OpenLockerWebApi.DTOs.Blob
{
    /// <summary>
    /// File Information to be returned back to the user
    /// </summary>
    public class File
    {
        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// DateTime when the file was last modified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Size of the file
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Content Type of the file
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Time when the file was first uploaded
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}