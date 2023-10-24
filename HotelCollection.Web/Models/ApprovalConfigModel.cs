using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Web.Models
{
    public class ApprovalConfigModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string ApprovalType { get; set; }
        public string Department { get; set; }
        public int ApprovalLevel { get; set; }
        public bool IsFinalLevel { get; set; }

    
    }
}
