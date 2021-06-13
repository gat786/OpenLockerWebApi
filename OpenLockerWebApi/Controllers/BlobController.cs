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
    /// <summary>
    /// A controller to Control all the api calls related to blob storage
    /// Authentication is required for all the calls intended to be done through this controller.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("blob")]
    public class BlobController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBlobService _blobService;

        /// <summary>
        /// Constructor loading userservice and blobservice from the DI controller
        /// DI -> Dependency Injection
        /// </summary>
        /// <param name="userService">Implementation of IUserService</param>
        /// <param name="blobService">Implementation of IBlobService</param>
        public BlobController(IUserService userService, IBlobService blobService)
        {
            _userService = userService;
            _blobService = blobService;
        }

        /// <summary>
        /// Get files inside a folder. 
        /// </summary>
        /// <param name="getFilesDto">Get Files specific details</param>
        /// <remarks>
        /// How to Call:
        ///     GET /blob
        /// </remarks>
        /// <returns></returns>
        [Authorize]
        [HttpGet("")]
        public ActionResult GetFiles(GetFilesDto getFilesDto)
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            var response = new StandardResponse
            {
                Success = true,
                Data = _blobService.GetFiles(containerClient, getFilesDto.Prefix)
            };
            return new OkObjectResult(response);
        }

        [Authorize]
        [HttpGet("all")]
        public ActionResult GetAllFiles(){
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            var response = new StandardResponse{
                Success = true,
                Data = _blobService.GetAllFiles(containerClient)
            };
            return new OkObjectResult(response);
        }

        /// <summary>
        /// Get Upload URI for a file
        /// </summary>
        /// <remarks>
        /// How to Call:
        ///     POST /blob/upload
        /// Sample Request: 
        ///     {
        ///         "FileName":"path/to/file/filename.jpg"
        ///     }
        /// </remarks>
        /// <param name="uploadFileUriProps">Props required to fulfil this request</param>
        /// <returns>Uri which can be used in a put request with file as a body to upload the file to storage</returns>
        [Authorize]
        [HttpPost("upload")]
        public ActionResult GetUploadFileUri(GetUploadFileUriRequest uploadFileUriProps)
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            var uploadUri = _blobService.GetUploadSasUri(client: containerClient, fileName: uploadFileUriProps.FileName);
            return new OkObjectResult($"{uploadUri.Scheme}://{uploadUri.Host}:{uploadUri.Port}{uploadUri.LocalPath}{uploadUri.Query}");
        }

        /// <summary>
        /// Route to get Download uri for a file from the blob storage
        /// </summary>
        /// <remarks>
        /// How to Call:
        ///     POST /blob/download
        /// Sample Request:
        ///     {
        ///         "FileName":"path/to/file/filename.jpg"
        ///     }
        /// </remarks>
        /// <param name="downloadFileDto">Props required to fulfil this request</param>
        /// <returns>Download uri that can be used to download specified file</returns>
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

        /// <summary>
        /// API endpoint to Delete a particular blob
        /// </summary>
        /// <remarks>
        /// How to Call:
        ///     POST /blob/delete
        /// Sample Request:
        ///     {
        ///         "FileName":"path/to/file/filename.jpg"
        ///     }
        /// </remarks>
        /// <param name="deleteBlobDto"></param>
        /// <returns></returns>
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
