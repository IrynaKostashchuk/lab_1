namespace webAPI.dal.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository _authorRepository { get; }
        IBookRepository _bookRepository { get; }
        ILoanRepository _loanRepository { get; }
        IReaderRepository _readerRepository { get; }
       
        void Commit();
        void Dispose();
    }
}
