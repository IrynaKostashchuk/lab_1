using System.Data;
using System.Data.SqlClient;
using Dapper;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.dal.Repositories;

public class BookRepository: GenericRepository<Book>, IBookRepository
{
    public BookRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction, string tableName) : base(sqlConnection, dbTransaction, tableName)
    {
    }

    public async Task<IEnumerable<Book>> TopThreeBookAsync()
    {
        string sql = @"SELECT TOP 3 * FROM Book";
        var results = await _sqlConnection.QueryAsync<Book>(sql,
            transaction: _dbTransaction);
        return results;
    }
}