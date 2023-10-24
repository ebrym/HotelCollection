using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Data.Entity
{
    public class Approval:BaseEntity.Entity
    {
        public string Comment { get; set; }
        public bool ApprovalStatus { get; set; }
        public int ApprovalLevel { get; set; }
        // public virtual Role ApproverRoleId { get; set; }
        public ApplicationUser ApproverUserId { get; set; }
        public int RequisitionId {get;set;}
        public int  ApprovalConfigId { get; set; }
    }
}
