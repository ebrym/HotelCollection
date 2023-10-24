using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Data.Entity
{
   public class Requisition : BaseEntity.Entity
    {
        public string StaffName { get; set; }
        public string StaffEmail { get; set; }
        public string StaffNumber { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Unit { get; set; }
        public string RequisitionType { get; set; }
        public string ApprovalStatus { get; set; }
        public string CurrentApprovalLevel { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string MarketerName { get; set; }
        public string DocumentPath { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public ICollection<RequisitionDetails> RequisitionDetails { get; set; }
    }
}
