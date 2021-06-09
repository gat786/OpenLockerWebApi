﻿using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OpenLockerWebApi.Models;
using OpenLockerWebApi.Services.UserService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Services.BlobService
{
    public class BlobServiceImpl : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        private readonly IUserService _userService;

        public BlobServiceImpl(BlobServiceClient client, IUserService userService)
        {
            _blobClient = client;
            _userService = userService;
        }

        public BlobContainerClient CreateContainerForUser(User user)
        {
            string containerName = user.Username.ToLower();
            try
            {
                var container = _blobClient.CreateBlobContainer(containerName);
                return container.Value;
            }
            catch (RequestFailedException requestFailedException)
            {
                Debug.WriteLine(requestFailedException.Message);
                return null;
            }

        }

        public BlobContainerClient GetContainerForUser(User user)
        {
            _blobClient.GetBlobContainerClient(user.Username.ToLower()).CreateIfNotExists();
            var client = _blobClient.GetBlobContainerClient(user.Username.ToLower());
            return client;
        }

        public string GetDownloadUrl(BlobContainerClient client, string filePath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlobItem> GetFiles(BlobContainerClient client, string prefix = "")
        {
            var blobs = client.GetBlobsByHierarchy(prefix: prefix, delimiter: "/") as IEnumerable<BlobHierarchyItem>;
            var blobItems = blobs.Select(blobHierarchyItem => blobHierarchyItem.Blob);
            return blobItems;
        }
    }
}
