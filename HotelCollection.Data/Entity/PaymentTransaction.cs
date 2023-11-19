namespace HotelCollection.Data.Entity;

public class PaymentTransaction:BaseEntity.Entity
{
    public virtual PaymentSetup PaymentSetup { get; set; }
    public int PaymentSetupId { get; set; }
    public string Reference { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMode { get; set; }

    public string Status { get; set; }
}