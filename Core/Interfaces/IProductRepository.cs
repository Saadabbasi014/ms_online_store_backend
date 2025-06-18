using Core.Entites;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string? brand, string? type, string? sort);
        Task<IReadOnlyList<string>> GetBrandsAsync();
        Task<IReadOnlyList<string>> GetTypesAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<bool> ExistsAsync(int id);
        void UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
