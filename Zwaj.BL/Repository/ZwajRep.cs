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

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var message = context.Messages.Include(m => m.Sender).ThenInclude(u => u.Photos)
                .Include(m => m.Recipient).ThenInclude(u => u.Photos).AsQueryable();
            switch (messageParams.MessageType)
            {
                case "Inbox":
                    message = message.Where(m => m.RecipientId == messageParams.UserId);
                    break;
                case "Outbox":
                    message = message.Where(m => m.SenderId == messageParams.UserId);
                    break;
                    //unresad
                default:
                    message = message.Where(m => m.RecipientId == messageParams.UserId&& m.IsRead ==false);
                    break;
            }
            message = message.OrderByDescending(m => m.MessageSent);
            return await PagedList<Message>.GreateAsync(message, messageParams.PageNumber, messageParams.PageSize); 
        }

        public async Task<IEnumerable<Message>> GetConversation(string userId, string recipientId)
        {
            var message = await context.Messages.Include(m => m.Sender).ThenInclude(u => u.Photos)
               .Include(m => m.Recipient).ThenInclude(u => u.Photos).Where(u => u.RecipientId == userId && u.SenderId == recipientId || u.RecipientId == recipientId && u.SenderId == userId)
               .OrderByDescending(m=>m.MessageSent).ToListAsync();
            return message;
        }
    }
}
