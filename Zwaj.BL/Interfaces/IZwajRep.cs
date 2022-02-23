using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.Helper;
using Zwaj.DAL.Entity;
using Zwaj.DAL.Extend;

namespace Zwaj.BL.Interfaces
{
    public interface IZwajRep
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Edit<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(string id);
        Task<Photo> GetMainPhotoForUser(string userId);
        Task<Photo> GetPhoto(int id);
        Task<Like> GetLike(string userID,string likeeId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetConversation(string userId,string recipientId);

    }
}
