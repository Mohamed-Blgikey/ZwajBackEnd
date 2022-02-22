using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zwaj.BL.DTOs;
using Zwaj.BL.Helper;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.Entity;
using Zwaj.DAL.Extend;

namespace Zwaj.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await rep.GetUsers(userParams);
            var rusers = mapper.Map<IEnumerable<UserForListDTO>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
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

        [HttpPost]
        [Route("~/AddPhoto")]
        public async Task<IActionResult> AddPhotoToUser([FromBody] PhotoForUserDTO photoDto)
        {
            try
            {
                var main =  await rep.GetMainPhotoForUser(photoDto.UserId);
                if (main == null)
                {
                    photoDto.IsMain = true;
                }
                var photo = mapper.Map<Photo>(photoDto);
                rep.Add(photo);
                await rep.SaveAllAsync();
                return Ok(photo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("~/DeletePhoto")]
        public async Task<IActionResult> DeletePhoto([FromBody] DeletePhotoDTO photoDTO)
        {
            if (photoDTO.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Ok(new { message = "غير مسموح لك" });


            var userPhotoRepo = await rep.GetUser(photoDTO.UserId);

            if (!userPhotoRepo.Photos.Any(a => a.Id == photoDTO.PhotoId))
                return Ok(new { message = "لا توجد صوره لك" });


            var Photo = await rep.GetPhoto(photoDTO.PhotoId);

            if (!Photo.IsMain)
            {
                rep.Delete(Photo);
                await rep.SaveAllAsync();
                return Ok(new { message = "تم حذف الصوره" });
            }
            var newMain = userPhotoRepo.Photos.FirstOrDefault(p => p.IsMain == false);
            newMain.IsMain = true;
            rep.Delete(Photo);
            await rep.SaveAllAsync();
            return Ok(new { message = "تم حذف الصوره" });
        }



        [HttpPost]
        [Route("~/SetMain")]
        public async Task<IActionResult> SetMainPhoto(MainPhotoDTO photoDTO)
        {
            if (photoDTO.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Ok(new { message = "غير مسموح لك" });


            var userPhotoRepo = await rep.GetUser(photoDTO.UserId);

            if (!userPhotoRepo.Photos.Any(a=>a.Id == photoDTO.PhotoId))
                return Ok(new {message = "لا توجد صوره لك" });


            var desiredmainPhoto = await rep.GetPhoto(photoDTO.PhotoId);

            if (desiredmainPhoto.IsMain)
                return Ok(new {message = "هذه الصوره الاساسيه بالفغل" });

            var currentPhoto = await rep.GetMainPhotoForUser(photoDTO.UserId);

            currentPhoto.IsMain = false;
            desiredmainPhoto.IsMain = true;
            await rep.SaveAllAsync();
            return Ok(new { message = "تم التعديل" });

        }


        [HttpPost]
        [Route("~/{userId}/Like/{likeeId}")]
        public async Task<IActionResult> Like(string userId,string likeeId)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            var like = await rep.GetLike(userId,likeeId);
            if (like != null)
                return BadRequest(new { message = "لقد قمت بالاعجاب من قبل " });
            if (await rep.GetUser(likeeId) == null)
                return NotFound(new { message = "غير موجوود" });
            like = new Like
            {
                LikerId = userId,
                LikeeId = likeeId
            };
            rep.Add(like);
            await rep.SaveAllAsync();
            return Ok(new {message = "تم الاعجاب"});
        }
    }
}
