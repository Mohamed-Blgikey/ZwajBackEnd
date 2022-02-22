using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.DAL.Extend;

namespace Zwaj.DAL.Entity
{
    public class Like
    {
        public string LikerId { get; set; }
        public string LikeeId { get; set; }


        public User Liker { get; set; }
        public User Likee { get; set; }
    }
}
