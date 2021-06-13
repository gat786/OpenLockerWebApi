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

        public bool DeleteBlob(BlobContainerClient client, string filename)
        {
            try
            {
                client.DeleteBlobIfExists(filename, DeleteSnapshotsOption.IncludeSnapshots);
            }
            catch (RequestFailedException exception)
            {
                Debug.WriteLine(exception);
                return false;
            }
            return true;
        }

        public BlobContainerClient GetContainerForUser(User user)
        {
            _blobClient.GetBlobContainerClient(user.Username.ToLower()).CreateIfNotExists();
            var client = _blobClient.GetBlobContainerClient(user.Username.ToLower());
            return client;
        }

        public IEnumerable<File> GetAllFiles(BlobContainerClient client)
        {
            var blobItems = client.GetBlobs();
            var fileItems = blobItems.Select(blob => new File
            {
                FileName = blob.Name,
                ContentLength = blob.Properties.ContentLength ?? 0,
                ContentType = blob.Properties.ContentType,
                CreatedOn = blob.Properties.CreatedOn?.UtcDateTime ?? DateTime.MinValue,
                LastModified = blob.Properties.LastModified?.UtcDateTime ?? DateTime.MinValue
            });
            return fileItems;
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
            try
            {
                return blobClient.GenerateSasUri(builder);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

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
