using webAPI.dal.Entities;

namespace webAPI.dal.Repositories.Interfaces;

public interface IReaderRepository: IGenericRepository<Reader>
{
    Task<IEnumerable<Reader>> GetByLastNameAsync(string lastName);
}