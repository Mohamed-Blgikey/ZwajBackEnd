using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.DataBase;
using Zwaj.DAL.Extend;

namespace Zwaj.BL.Repository
{
    public class ZwajRep:IZwajRep
    {
        private readonly AppDbContext context;
        public ZwajRep(AppDbContext context)
        {
            this.context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            this.context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            this.context.Remove(entity);
        }

        public void Edit<T>(T entity) where T : class
        {
            this.context.Set<T>().Update(entity);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 1;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = this.context.Users.Include(u => u.Photos).Select(a=>a);
            return users;
        }
        public async Task<User> GetUser(string id)
        {
            var user = await this.context.Users.Include(u => u.Photos).FirstOrDefaultAsync(a => a.Id == id);
            return user;
        }
    }
}
