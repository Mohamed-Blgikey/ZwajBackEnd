using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.DAL.Extend;

namespace Zwaj.BL.Interfaces
{
    public interface IZwajRep
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string id);

    }
}
