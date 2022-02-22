using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.Helper;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.DataBase;
using Zwaj.DAL.Entity;
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
            context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public void Edit<T>(T entity) where T : class
        {
            context.Set<T>().Update(entity);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 1;
        }
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = context.Users.Include(u => u.Photos);
            return await PagedList<User>.GreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Photo> GetMainPhotoForUser(string userId)
        {
            return await context.MyProperty.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);

        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await context.MyProperty.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }
        public async Task<User> GetUser(string id)
        {
            var user = await this.context.Users.Include(u => u.Photos).FirstOrDefaultAsync(a => a.Id == id);
            return user;
        }

        public async Task<Like> GetLike(string userID, string likeeId)
        {
            return await context.Likes.FirstOrDefaultAsync(l=>l.LikerId == userID && l.LikeeId == likeeId);
        }
        private async Task<IEnumerable<string>> GetUserLikes(bool likers)
        {
            var user = await context.Users.Include(a => a.Likers).Include(a => a.Likees).FirstOrDefaultAsync();
            if (likers)
            {
                return user.Likers.Select(a=>a.LikerId);
            }
            else
            {
                return user.Likees.Select(a => a.LikeeId);
            }
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Messages.FirstOrDefaultAsync(m => m.id == id);
        }

        public Task<PagedList<Message>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetConversation(string userId, string recipientId)
        {
            throw new NotImplementedException();
        }
    }
}
