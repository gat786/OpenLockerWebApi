using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLockerWebApi.DTOs.User;
using OpenLockerWebApi.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Controllers
{
    [ApiController]
    [Route("blob")]
    public class BlobController
    {
        private readonly IUserService service;

        BlobController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public string CheckingAuth()
        {
            return "Done";
        }
    }
}
