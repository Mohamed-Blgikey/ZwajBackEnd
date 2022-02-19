using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zwaj.BL.DTOs;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.Extend;

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
        [Route("~/EditUser")]
        public async Task<IActionResult> EditUser(UserForUpdateDTO user)
        {
            if (user.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var ruser = await this.rep.GetUser(user.Id);
            ruser.Introduction = user.Introduction;
            ruser.LookingFor = user.LookinFor;
            ruser.Interests = user.Interests;
            ruser.City = user.City;
            ruser.Country = user.Country;
            rep.Edit(ruser);
            await rep.SaveAllAsync();
                return Ok(new {message = "تم التعديل" });
        }

        [Route("~/SavePhoto")]
        [HttpPost]
        public async Task<IActionResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = Guid.NewGuid() + postedFile.FileName;
                var physicalPath = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }

                return Ok(new { message = filename });
            }
            catch (Exception)
            {
                return Ok(new { message = "Error !" });
            }
        }

        [HttpPost]
        [Route("~/UnSavePhoto")]
        public JsonResult UnSaveFile([FromBody] PhotoDTO photoVM)
        {
            try
            {
                if (System.IO.File.Exists(Directory.GetCurrentDirectory() + "/wwwroot/Img/" + photoVM.Name))
                {
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/Img/" + photoVM.Name);
                }

                return new JsonResult(new { message = "Deleted !" });
            }
            catch (Exception)
            {

                return new JsonResult("Error!");
            }
        }
    }
}
