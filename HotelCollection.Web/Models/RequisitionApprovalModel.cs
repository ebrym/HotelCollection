using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Web.Models
{
    public class RequisitionApprovalModel
    {
        public int Id { get; set; }
       
        public string CurrentApprovalLevel { get; set; }
       
        public string ApprovalComments { get; set; }
    }
}
