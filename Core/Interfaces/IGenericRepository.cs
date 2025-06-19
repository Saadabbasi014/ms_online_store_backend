using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T?>> GetListAsync(ISpecification<T> spec);
        Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
        Task<IReadOnlyList<TResult?>> GetListAsync<TResult>(ISpecification<T, TResult> spec);
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);    
        Task<bool> ExistsAsync(int id); 
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
