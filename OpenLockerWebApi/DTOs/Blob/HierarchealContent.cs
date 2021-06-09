using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.Blob
{
    /// <summary>
    /// Response Data type to be send when all the files of a particular folder is requested
    /// </summary>
    public class HierarchealContent
    {
        /// <summary>
        /// Prefix Folder
        /// </summary>
        public IEnumerable<string> FolderPrefix { get; set; }

        /// <summary>
        /// File item for each file in the current directory
        /// </summary>
        public IEnumerable<File> Files { get; set; }

        /// <summary>
        /// Load current class from HierachialItem Response
        /// </summary>
        /// <param name="items">BlobHierarchyItem</param>
        public void FromBlobHierarchyItem(List<BlobHierarchyItem> items)
        {
            var blobs = items.FindAll(item => item.IsBlob);
            var prefixes = items.FindAll(item => item.IsPrefix);
            this.FolderPrefix = prefixes.Select(prefix => prefix.Prefix);
            this.Files = blobs.Select(blob => new File
            {
                FileName = blob.Blob.Name,
                ContentLength = blob.Blob.Properties.ContentLength ?? 0,
                ContentType = blob.Blob.Properties.ContentType,
                CreatedOn = blob.Blob.Properties.CreatedOn?.UtcDateTime ?? DateTime.MinValue,
                LastModified = blob.Blob.Properties.LastModified?.UtcDateTime ?? DateTime.MinValue
            });
        }
    }
}
