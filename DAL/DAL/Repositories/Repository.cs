using DAL.Data;
using DAL.Utilities; // Add this using directive
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly WebAppContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(WebAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null)
            {
                throw new InvalidOperationException($"Entity with id {id} not found.");
            }
            return entity;
        }

        public async Task AddAsync(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null)
            {
                throw new InvalidOperationException($"Entity with id {id} not found.");
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate)
        {
            _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entity is null)
            {
                throw new InvalidOperationException("Entity not found.");
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
