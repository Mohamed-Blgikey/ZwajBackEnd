using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zwaj.BL.DTOs;
using Zwaj.BL.Helper;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.Entity;

namespace Zwaj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRep rep;
        private readonly IOptions<JWT> jwt;

        public AuthController(IAuthRep rep,IOptions<JWT> jwt)
        {
            this.rep = rep;
            this.jwt = jwt;
        }

        [HttpPost]
        [Route("~/Register")]
        public async Task<IActionResult> Register(RegisterDto data)
        {
            data.UserName = data.UserName.ToLower();
            if (await rep.UserExsit(data.UserName))
            {
                return BadRequest(new {message = "هذا المستخدم مسجل من قبل" });
            }

            var userToCreate = new User
            {
                UserName = data.UserName,
            };
            var user = await rep.Register(userToCreate, data.Password);
            return Ok(new { message = "تم تسجيل مستخدم جديد" });
        }

        [HttpPost]
        [Route("~/Login")]
        public async Task<IActionResult> Login(LoginDto data)
        {
            var user = await rep.Login(data.UserName.ToLower() , data.Password);
            if (user == null)
                return Unauthorized(new {message = "ليس لك صلاحيه بالدخول"});

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
    }
}
