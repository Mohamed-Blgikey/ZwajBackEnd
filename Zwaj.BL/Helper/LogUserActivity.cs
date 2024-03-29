﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.Interfaces;

namespace Zwaj.BL.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var repo = resultContext.HttpContext.RequestServices.GetService<IZwajRep>();
            var user = await repo.GetUser(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}
