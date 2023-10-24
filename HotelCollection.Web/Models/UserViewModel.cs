using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }

    }
    public class UserRoleViewModel
    {
        public bool SelectedRole { get; set; }
        public string Role { get; set; }
    }
}
