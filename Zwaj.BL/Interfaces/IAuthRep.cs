using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.DTOs;
using Zwaj.BL.Helper;

namespace Zwaj.BL.Interfaces
{
    public interface IAuthRep
    {
        Task<AuthModel> Register(RegisterDto register);
        Task<AuthModel> Login(LoginDto login);
    }
}
