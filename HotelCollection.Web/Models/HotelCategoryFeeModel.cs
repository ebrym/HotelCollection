namespace HotelCollection.Web.Models
{
    public class HotelCategoryFeeModel
    {
        public int Id { get; set; }
        public double ? Fee { get; set; }   
        public HotelCategoryModel Category { get; set; }
        public int CategoryId { get; set; }
    }
}
