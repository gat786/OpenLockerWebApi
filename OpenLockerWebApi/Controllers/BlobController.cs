using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
    public class BlobController: ControllerBase
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
        public ActionResult GetFiles()
        {
            User user = _userService.GetUserByUsername(User.FindFirstValue(ClaimTypes.Name));
            var containerClient = _blobService.GetContainerForUser(user);
            return new OkObjectResult(_blobService.GetFiles(containerClient));
        }
    }
}
