using System.Data;
using System.Data.SqlClient;
using Dapper;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.dal.Repositories;

public class AuthorRepository: GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction, string tableName) : base(sqlConnection, dbTransaction, tableName)
    {
    }


    public async Task<IEnumerable<Author>> TopFiveAuthorAsync()
    {
        string sql = @"SELECT TOP 5 * FROM Author";
        var results = await _sqlConnection.QueryAsync<Author>(sql,
            transaction: _dbTransaction);
        return results;
    }
}