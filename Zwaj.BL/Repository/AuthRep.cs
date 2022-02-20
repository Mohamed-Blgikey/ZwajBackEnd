using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.DTOs;
using Zwaj.BL.Helper;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.DataBase;
using Zwaj.DAL.Extend;

namespace Zwaj.BL.Repository
{
    public class AuthRep : IAuthRep
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IOptions<JWT> jwt;

        public AuthRep(UserManager<User> userManager,RoleManager<IdentityRole> roleManager,IOptions<JWT> jwt)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwt = jwt;
        }

        public async Task<AuthModel> Login(LoginDto login)
        {
            var user  = await userManager.FindByEmailAsync(login.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user,login.Password))
                return new AuthModel { Message = "لايوجد بريد بهذا الاسم او كلمه السر غير صحيحه " };
            var token = await createJwtToken(user);
            return new AuthModel
            {
                Message = "تم تسجيل الدخول",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                IsAuthentcation = true,
                Expire = token.ValidTo
            };
        }

        public async Task<AuthModel> Register(RegisterDto register)
        {
            if (await userManager.FindByEmailAsync(register.Email) != null)
            {
                return new AuthModel { Message = "هذا البريد مستخدم من قبل" };
            }
            var user = new User
            {
                Email = register.Email,
                UserName = register.Email,
                Name = register.Name,
                Gender =register.Gender ,
                City = register.City ,
                Country =register.Country ,
                DateOfBirth = register.DateOfBirth,
                Interests = register.Interests,
                Introduction = register.Introduction,
                LookingFor = register.LookingFor
            };

            var result = await userManager.CreateAsync(user,register.Password);
            if (!result.Succeeded)
            {
                var error = string.Empty;
                foreach (var item in result.Errors)
                {
                    error += $"{item.Description} , ";
                }
                return new AuthModel { Message = error };
            }

            var RoleExsit = await roleManager.RoleExistsAsync("admin");
            if (!RoleExsit)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await userManager.AddToRoleAsync(user, "admin");
            }
            else
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
                await userManager.AddToRoleAsync(user, "user");
            }
            var token = await createJwtToken(user);
            return new AuthModel { Message = "تم تسجيل مستخدم جديد",UserId = user.Id ,Token = new JwtSecurityTokenHandler().WriteToken(token) ,IsAuthentcation = true,Expire = token.ValidTo};
        }

        private async Task<JwtSecurityToken> createJwtToken(User user)
        {
            var userClaim = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var item in userRoles)
            {
                roleClaims.Add(new Claim("Roles",item));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id),
                new Claim(JwtRegisteredClaimNames.Sub,user.Name),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
            }
            .Union(userClaim)
            .Union(roleClaims);

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.Key));
            var cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Value.Issuer,
                audience: jwt.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwt.Value.DurationInDay),
                signingCredentials: cred
                );
            return jwtSecurityToken;
        }
    }
}
