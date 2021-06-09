﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OpenLockerWebApi.DTOs.Blob;
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
        BlobContainerClient CreateContainerForUser(User user);

        /// <summary>
        /// Get Container in which user files are stored
        /// </summary>
        /// <param name="user">User for which Container is requested</param>
        /// <returns></returns>
        BlobContainerClient GetContainerForUser(User user);

        /// <summary>
        /// Gets Root folder for the specified user
        /// </summary>
        /// <param name="client">Blob Container Client for the appropriate container</param>
        /// <param name="prefix">Prefix from where you want to list your files</param>
        /// <returns>List of files and prefixes in the given folder</returns>
        HierarchealContent GetFiles(BlobContainerClient client,string prefix = "");

        /// <summary>
        /// Generates SAS token for the file with READ perms and returns url to download the
        /// file
        /// </summary>
        /// <param name="client">Blob Container Client for the appropriate container</param>
        /// <param name="filePath">File that you want to download</param>
        /// <returns>URL with appended token to download file</returns>
        Uri GetDownloadUrl(BlobContainerClient client,string filePath);

        /// <summary>
        /// Get Upload SasUri for a file
        /// </summary>
        /// <param name="client">Container client in which you have to upload the file</param>
        /// <param name="filePrefix">file prefix at which you want to upload the file</param>
        /// <param name="fileName">File Name for which you have to create upload uri</param>
        /// <returns>SasUri for the file which you can use to upload the file</returns>
        Uri GetUploadSasUri(BlobContainerClient client, string fileName = "");
    }
}
