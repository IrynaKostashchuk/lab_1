using webAPI.dal.Entities;

namespace webAPI.dal.Repositories.Interfaces;

public interface IBookRepository: IGenericRepository<Book>
{
    Task<IEnumerable<Book>> TopThreeBookAsync();
}