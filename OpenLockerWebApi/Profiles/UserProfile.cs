using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OpenLockerWebApi.DTOs.User;
using OpenLockerWebApi.Models;

namespace OpenLockerWebApi.Profiles
{
    public class UserProfile: AutoMapper.Profile
    {
        // <Destination, Source>
        public UserProfile()
        {
            CreateMap<User, UserRead>();
            CreateMap<UserCreate, User>();
            CreateMap<UserRead, User>();
        }
    }
}
