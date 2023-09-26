namespace webAPI.dal.Entities;

public class Book : BaseEntity
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public int PublishYear { get; set; }
    
    public int Quantity { get; set; }
    
}