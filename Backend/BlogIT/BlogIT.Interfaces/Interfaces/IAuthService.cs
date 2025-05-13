using BlogIT.DataTransferObjects;
using BlogIT.Model.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogIT.Interfaces.Interfaces
{
    public interface IAuthService
    {
        Task<bool> UserExists(string email, string username);
        Task<IdentityResult> RegisterUser(UserRegisterDto userRegisterDto);
    }
}
