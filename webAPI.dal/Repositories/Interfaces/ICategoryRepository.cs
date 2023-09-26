using webAPI.dal.Entities;

namespace webAPI.dal.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> TopFiveCategoryAsync();
    }
}
