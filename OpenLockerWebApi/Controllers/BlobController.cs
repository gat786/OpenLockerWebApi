using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Net.Http.Headers;
using OpenLockerWebApi.DTOs;
using OpenLockerWebApi.DTOs.Blob;
using OpenLockerWebApi.DTOs.User;
using OpenLockerWebApi.Models;
using OpenLockerWebApi.Services.BlobService;
using OpenLockerWebApi.Services.UserService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Controllers
{
    [ApiController]
    [Route("blob")]
    public class BlobController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBlobService _blobService;

        public BlobController(IUserService userService, IBlobService blobService)
        {
            _userService = userService;
            _blobService = blobService;
        }

        [Authorize]
        [HttpGet("")]
        public ActionResult GetFiles(GetFilesDto getFilesDto)
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            return new OkObjectResult(_blobService.GetFiles(containerClient,getFilesDto.Prefix));
        }

        [Authorize]
        [HttpPost("upload")]
        public ActionResult GetUploadFileUri(GetUploadFileUriRequest uploadFileUriProps)
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            var uploadUri = _blobService.GetUploadSasUri(client: containerClient, fileName: uploadFileUriProps.FileName);
            return new OkObjectResult($"{uploadUri.Scheme}://{uploadUri.Host}:{uploadUri.Port}{uploadUri.LocalPath}{uploadUri.Query}");
        }

        [Authorize]
        [HttpPost("download")]
        public ActionResult GetDownloadUrl(DownloadFileDto downloadFileDto)
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            var downloadUri = _blobService.GetDownloadUrl(containerClient, downloadFileDto.FileName);
            if (downloadUri != null)
            {
                var result = new StandardResponse
                {
                    Success = true,
                    Data = $"{downloadUri.Scheme}://{downloadUri.Host}:{downloadUri.Port}{downloadUri.LocalPath}{downloadUri.Query}"
                };
                return new OkObjectResult(result);
            }

            var failedResult = new StandardResponse
            {
                Success = false,
                Message = "Creation of the url failed. Check whether the file really exists or you passed correct details"
            };
            return new BadRequestObjectResult(failedResult);
        }

        [Authorize]
        [HttpPost("delete")]
        public ActionResult DeleteBlob(DeleteBlobDto deleteBlobDto)
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            var result = _blobService.DeleteBlob(containerClient, deleteBlobDto.FileName);
            return Ok(); 
        }
    }
}
