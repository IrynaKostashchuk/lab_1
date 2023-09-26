using webAPI.dal.Entities;

namespace webAPI.dal.Repositories.Interfaces;

public interface ILoanRepository: IGenericRepository<Loan>
{
    Task<IEnumerable<Loan>> LoanByReaderName(string name);
}