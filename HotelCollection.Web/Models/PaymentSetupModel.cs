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

    }
}
