

namespace webAPI.dal.Entities;

public class Reader : BaseEntity
{
    public int Id { get; set; }
    
    public string? ReaderFirstName { get; set; }
    
    public string? ReaderLastName { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? Address { get; set; }
}