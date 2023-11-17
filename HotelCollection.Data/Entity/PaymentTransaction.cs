namespace HotelCollection.Data.Entity;

public class PaymentTransaction:BaseEntity.Entity
{
    public string Reference { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMode { get; set; }

    public string Status { get; set; }
}