using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Data.Entity
{
    public class ApprovalConfig : BaseEntity.Entity
    {
        public int RoleId { get; set; }
        public string PaymentType { get; set; }
        public string ApprovalType { get; set; }
        public int ApprovalLevel { get; set; }
        public bool IsFinalLevel { get; set; }

    
    }
}
