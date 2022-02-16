using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.DAL.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        //key for PasswordHash
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
