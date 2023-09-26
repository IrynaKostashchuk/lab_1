using System.Data;
using System.Data.SqlClient;
using Dapper;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.dal.Repositories;

public class LoanRepository: GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction, string tableName) : base(sqlConnection, dbTransaction, tableName)
    {
    }

    public async Task<IEnumerable<Loan>> LoanByReaderName(string name)
    {
        string sql = @"
        SELECT L.*
        FROM Loan AS L
        INNER JOIN Reader AS R ON L.ReaderID = R.Id
        WHERE CONCAT(R.ReaderFirstName, ' ', R.ReaderLastName) = @ReaderName
    ";
    
        var results = await _sqlConnection.QueryAsync<Loan>(sql,
            param: new { ReaderName = name },
            transaction: _dbTransaction);

        return results;
    }
}