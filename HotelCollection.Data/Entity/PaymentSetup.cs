using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Data.Entity
{
    public class PaymentSetup:BaseEntity.Entity
    {
        public virtual Hotel Hotel { get; set; }
        public int HotelId { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public int PaymentTypeId { get; set; }
        public double Amount { get; set; }
       public  string ReferenceNo { get; set; }
       public string ApprovalStatus { get; set; }
       public string CurrentApprovalLevel { get; set; }
       
    }
}
