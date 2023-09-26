using System.Data;
using System.Data.SqlClient;
using Dapper;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.dal.Repositories;

public class ReaderRepository: GenericRepository<Reader>, IReaderRepository
{
    public ReaderRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction, string tableName) : base(sqlConnection, dbTransaction, tableName)
    {
    }

    public async Task<IEnumerable<Reader>> GetByLastNameAsync(string lastName)
    {
        string sql = "SELECT * FROM Reader WHERE ReaderLastName = @LastName";
        var results = await _sqlConnection.QueryAsync<Reader>(sql,
            param: new { LastName = lastName },
            transaction: _dbTransaction);

        return results;
    }
}