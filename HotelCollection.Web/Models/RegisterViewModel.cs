using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Web.Models
{
    public class RegisterViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }
        
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string JobTitle { get; set; }
    }
}
