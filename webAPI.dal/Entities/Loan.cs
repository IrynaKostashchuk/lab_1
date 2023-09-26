namespace webAPI.dal.Entities;

public class Loan
{
    public int Id { get; set; }
    public int ReaderID { get; set; }
    public int BookID { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime ReturnDate { get; set; }
}