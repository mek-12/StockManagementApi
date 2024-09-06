
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StockManagement.DAL.Interfces;

namespace StockManagement.DAL.Concrete {
    public class Repository<T> : IRepository<T> where T : class {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context) {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

        public async Task<T> FindAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public void Update(T entity) => _dbSet.Update(entity);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();
    }
}
