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
        string CreateStorageFolderForUser(User user);

        /// <summary>
        /// Gets Root folder for the specified user
        /// </summary>
        /// <param name="user">User for which you have to create a storage folder</param>
        /// <returns>Name of the root folder</returns>
        string GetStorageFolderForUser(User user);
    }
}
