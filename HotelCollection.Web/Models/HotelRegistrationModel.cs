namespace HotelCollection.Web.Models;

public class HotelRegistrationModel
{
    public int Id { get; set; }
    public string Name  { get; set; }
    public string Address  { get; set; }
    public string Phone  { get; set; }
    public string Email  { get; set; }
    public string CACNumber  { get; set; }
    public HotelCategoryModel Category{ get; set; }
    public int CategoryId { get; set; }
}