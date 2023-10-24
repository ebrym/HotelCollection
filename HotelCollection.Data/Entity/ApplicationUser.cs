using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HotelCollection.Data.Entity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       //public string UserName { get; set; }
        //public string Email { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Unit { get; set; }
        public string LineManager { get; set; }
        public bool IsAdmin { get; set; }


        public virtual ICollection<IdentityUserRole<int>> Roles { get; set; } = new List<IdentityUserRole<int>>();
        [NotMapped]
        public string FullName
        {
            get
            {
                return this.LastName + " " + this.FirstName;
            }
        }
    }
}
