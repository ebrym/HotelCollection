using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Repository.Dtos
{
    public class ApprovalDto
    {
        public string Comment { get; set; }
        public bool ApprovalStatus { get; set; }
        public int ApprovalLevel { get; set; }
        // public virtual Role ApproverRoleId { get; set; }
        public string ApprovalFullName { get; set; }
        public int RequisitionId {get;set;}
        public int  ApprovalConfigId { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}
