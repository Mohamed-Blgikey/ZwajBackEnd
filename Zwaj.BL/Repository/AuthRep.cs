using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.DataBase;
using Zwaj.DAL.Entity;

namespace Zwaj.BL.Repository
{
    public class AuthRep : IAuthRep
    {
        private readonly AppDbContext context;

        public AuthRep(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<User> Login(string UserName, string Password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=>u.UserName == UserName);
            if (user == null)
                return null;
            if (!VerfiyPasswordHash(Password, user.PasswordSalt, user.PasswordHash))
                return null;
            return user;
        }

        private bool VerfiyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < ComputedHash.Length; i++)
                {
                    if (ComputedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string Password)
        {
            byte[] passwordHash, passwordSalt;
            createPassHash(Password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        private void createPassHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExsit(string UserName)
        {
            return await context.Users.AnyAsync(x => x.UserName == UserName);
        }
    }
}
