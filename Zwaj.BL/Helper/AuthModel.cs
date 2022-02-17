using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.BL.Helper
{
    public class AuthModel
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public bool IsAuthentcation { get; set; }
        public DateTime Expire { get; set; }
    }
}
