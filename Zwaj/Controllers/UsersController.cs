using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [HttpPut]
        [Route("~/EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id,UserForUpdateDTO dTO)
        {
            if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            var user = await this.rep.GetUser(id);
            user.Introduction = dTO.Introduction;
            user.LookingFor = dTO.LookinFor;
            user.Interests = dTO.Interests;
            user.City = dTO.City;
            user.Country = dTO.Country;
            await rep.SaveAllAsync();
                return Ok(new {message = "تم التعديل" });
        }
    }
}
