namespace webAPI.dal.Entities;

public class Author : BaseEntity
{
    public int Id { get; set; }

    public string? AuthorFirstName { get; set; }
    
    public string? AuthorLastName { get; set; }
    
    public int AuthorBirthYear { get; set; }
}