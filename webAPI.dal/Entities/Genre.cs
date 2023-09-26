namespace webAPI.dal.Entities;

public class Genre : BaseEntity
{
    public int GenreID { get; set; }
    
    public string? GenreName { get; set; }
}