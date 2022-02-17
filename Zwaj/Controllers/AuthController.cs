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

namespace Zwaj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRep rep;

        public AuthController(IAuthRep rep)
        {
            this.rep = rep;
        }

        [HttpPost]
        [Route("~/Register")]
        public async Task<IActionResult> Register(RegisterDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await rep.Register(data);
            if (!result.IsAuthentcation)
                return Ok(new { message = result.Message });

            return Ok(result);
        }

        [HttpPost]
        [Route("~/Login")]
        public async Task<IActionResult> Login(LoginDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await rep.Login(data);
            if (!result.IsAuthentcation)
                return Ok(new { message = result.Message });

            return Ok(result);
        }
    }
}
