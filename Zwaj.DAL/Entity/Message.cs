using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.DAL.Extend;

namespace Zwaj.DAL.Entity
{
    public class Message
    {
        public int id { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string RecipientId { get; set; }
        public User Recipient { get; set; }
        public int Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }

        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
