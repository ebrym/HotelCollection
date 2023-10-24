using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Web.Models
{
    public class ApprovalModel
    {
        public string Comment { get; set; }
        public bool ApprovalStatus { get; set; }
        public int ApprovalLevel { get; set; }
        public string ApprovalFullName { get; set; }
        public int RequisitionId {get;set;}
        public int  ApprovalConfigId { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}
