using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Services.BlobService
{
    public interface IBlobService
    {
        /// <summary>
        /// Every user will have a common root folder which will be considered as their locker
        /// </summary>
        /// <param name="user">User for which you have to create a storage folder</param>
        /// <returns>Name of the root folder</returns>
        string CreateContainerForUser(User user);

        /// <summary>
        /// Get Container in which user files are stored
        /// </summary>
        /// <param name="user">User for which Container is requested</param>
        /// <returns></returns>
        BlobContainerClient GetContainerForUser(User user);

        /// <summary>
        /// Gets Root folder for the specified user
        /// </summary>
        /// <param name="user">User for which you have to create a storage folder</param>
        /// <param name="prefix">Prefix from where you want to list your files</param>
        /// <returns>List of BlobItem in the given folder</returns>
        IEnumerable<BlobItem> GetFiles(BlobContainerClient client,string prefix);

        /// <summary>
        /// Generates SAS token for the file with READ perms and returns url to download the
        /// file
        /// </summary>
        /// <param name="client">Blob Container Client for which you have to download files</param>
        /// <param name="filePath">File that you want to download</param>
        /// <returns>URL with appended token to download file</returns>
        string GetDownloadUrl(BlobContainerClient client,string filePath);
    }
}
