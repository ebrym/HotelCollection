namespace HotelCollection.Data.Entity;

public class Agent: BaseEntity.Entity
{
    public string Name  { get; set; }
    public string Address  { get; set; }
    public string Phone  { get; set; }
    public string Email  { get; set; }
}