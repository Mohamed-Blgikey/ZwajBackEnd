using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.BL.DTOs
{
    public class RegisterDto
    {
       
        public string Name { get; set; }
        public string Gender { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Interests { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        
        public DateTime DateOfBirth { get; set; }
    }
}
