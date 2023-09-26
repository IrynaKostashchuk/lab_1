using System.Data;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.dal.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IAuthorRepository _authorRepository { get; }
        
        public IBookRepository _bookRepository { get; }
        
        public ILoanRepository _loanRepository { get; }
        
        public IReaderRepository _readerRepository { get; }

        readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IDbTransaction dbTransaction, IAuthorRepository authorRepository,
            IBookRepository bookRepository, ILoanRepository loanRepository, IReaderRepository readerRepository)
        {
            _dbTransaction = dbTransaction;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _readerRepository = readerRepository;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                // By adding this we can have muliple transactions as part of a single request
                //_dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            //Close the SQL Connection and dispose the objects
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
