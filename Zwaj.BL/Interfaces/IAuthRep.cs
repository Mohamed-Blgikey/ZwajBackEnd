using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.DAL.Entity;

namespace Zwaj.BL.Interfaces
{
    public interface IAuthRep
    {
        Task<User> Register(User user,string Password);
        Task<User> Login(string UserName,string Password);
        Task<bool> UserExsit(string UserName);
    }
}
