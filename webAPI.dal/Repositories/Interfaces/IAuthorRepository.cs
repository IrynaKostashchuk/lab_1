using webAPI.dal.Entities;

namespace webAPI.dal.Repositories.Interfaces;

public interface IAuthorRepository: IGenericRepository<Author>
{
    Task<IEnumerable<Author>> TopFiveAuthorAsync();

}