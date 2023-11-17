namespace HotelCollection.Web.Models;

public class PaymentModel
{
    public string Reference { get; set; }
    public string HotelName { get; set; }
    public double Amount { get; set; }
    public string Email { get; set; }
    public string PaymentType { get; set; }
}