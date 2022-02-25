using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Zwaj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("usersWithRoles")]
        public IActionResult get()
        {
            return Ok("مصرح للمدير فقط");
        }
    }
}
