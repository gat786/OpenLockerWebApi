using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using OpenLockerWebApi.DTOs.Blob;
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

        public Uri GetDownloadUrl(BlobContainerClient client, string fileName)
        {
            var blobClient = client.GetBlobClient(fileName);
            BlobSasBuilder builder = new BlobSasBuilder()
            {
                BlobContainerName = client.Name,
                BlobName = fileName,
                Resource = "b"
            };
            builder.ExpiresOn = DateTime.UtcNow.AddHours(2);
            builder.SetPermissions(BlobSasPermissions.Read);
            return blobClient.GenerateSasUri(builder);
        }

        public HierarchealContent GetFiles(BlobContainerClient client, string prefix = "")
        {
            var blobs = client.GetBlobsByHierarchy(prefix: prefix, delimiter: "/") as IEnumerable<BlobHierarchyItem>;
            var hirarchealData = new HierarchealContent();
            hirarchealData.FromBlobHierarchyItem(blobs.ToList());
            return hirarchealData;
        }

        public Uri GetUploadSasUri(BlobContainerClient client, string fileName = "")
        {
            var blobClient = client.GetBlobClient(fileName);
            if (blobClient != null && blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder builder = new()
                {
                    BlobContainerName = client.Name,
                    BlobName = fileName,
                    Resource = "b"
                };
                builder.ExpiresOn = DateTime.UtcNow.AddHours(2);
                builder.SetPermissions(BlobSasPermissions.Create | BlobSasPermissions.Write);
                return blobClient.GenerateSasUri(builder);
            }
            return null;
        }
    }
}
