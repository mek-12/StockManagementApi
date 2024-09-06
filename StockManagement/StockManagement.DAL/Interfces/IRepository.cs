using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StockManagement.DAL.Interfces {
    public interface IRepository<T> where T : class {
        Task<T> FindAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity); // Entity güncelleme
        Task<int> SaveChangesAsync();
        IQueryable<T> AsQueryable();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
