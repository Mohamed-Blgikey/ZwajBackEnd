using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.BL.DTOs
{
    public class MessageToReturnDTO
    {
        public int id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhotoUrl { get; set; } 
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
