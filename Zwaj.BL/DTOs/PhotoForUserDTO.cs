using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.BL.DTOs
{
    public class PhotoForUserDTO
    {
      
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public string UserId { get; set; }
    }
}
