using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task Delete(T entity);
        Task SaveAsync();
    }
}
