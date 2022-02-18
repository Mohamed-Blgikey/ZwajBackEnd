using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zwaj.BL.DTOs;
using Zwaj.BL.Interfaces;

namespace Zwaj.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IZwajRep rep;
        private readonly IMapper mapper;

        public UsersController(IZwajRep rep,IMapper mapper)
        {
            this.rep = rep;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("~/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this.rep.GetUsers();
            var rusers = mapper.Map<IEnumerable<UserForListDTO>>(users);
            return Ok(rusers);
        }

        [HttpGet]
        [Route("~/GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await this.rep.GetUser(id);
            var ruser = mapper.Map<UserForDetailsDTO>(user);
            return Ok(ruser);
        }
    }
}
