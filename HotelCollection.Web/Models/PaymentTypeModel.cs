namespace HotelCollection.Web.Models
{
    public class PaymentTypeModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public HotelCategoryModel Category { get; set; }
        public int CategoryId { get; set; }
    }
}
