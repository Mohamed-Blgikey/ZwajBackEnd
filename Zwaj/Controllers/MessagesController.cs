using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zwaj.BL.Helper;
using Zwaj.BL.Interfaces;

namespace Zwaj.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IZwajRep rep;
        private readonly IMapper mapper;

        public MessagesController(IZwajRep rep,IMapper mapper)
        {
            this.rep = rep;
            this.mapper = mapper;
        }
        [HttpGet("{id}",Name ="GetMessage")]
        public async Task<IActionResult> GetMessage(string userId,int id)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var messageFromRepo = await rep.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();
            return Ok(messageFromRepo);
        }
    }
}