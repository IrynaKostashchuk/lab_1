using webAPI.dal.Entities;

namespace webAPI.dal.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> ProductByCategoryAsync(int CategoryId);
    }
}
