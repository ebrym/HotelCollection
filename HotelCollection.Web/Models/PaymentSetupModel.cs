using System;

namespace HotelCollection.Web.Models
{
    public class PaymentSetupModel
    {
        public int Id { get; set; }
        public HotelRegistrationModel Hotel { get; set; }
        public int HotelId { get; set; }
        public PaymentTypeModel PaymentType { get; set; }
        public int PaymentTypeId { get; set; }
        public double Amount { get; set; }
        public string ReferenceNo { get; set; }
        public string ApprovalStatus { get; set; }
        public string CurrentApprovalLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public string  ApprovalComments { get; set; }
        public bool  Approved { get; set; }
        public string  AppLink { get; set; }

    }
}
