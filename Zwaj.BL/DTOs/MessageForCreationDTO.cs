using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.BL.DTOs
{
    public class MessageForCreationDTO
    {
        public MessageForCreationDTO()
        {
            MessageSent = DateTime.Now;
        }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime MessageSent { get; set; }

    }
}
