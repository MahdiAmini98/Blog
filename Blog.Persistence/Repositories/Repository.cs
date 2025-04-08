using Blog.Domain.Interfaces;
using Blog.Domain.Specifications;
using Blog.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Add a new entity
        public void Add(T entity) => _dbSet.Add(entity);

        // Remove an entity
        public void Remove(T entity) => _dbSet.Remove(entity);

        // Update an existing entity
        public void Update(T entity) => _dbSet.Update(entity);

        // Find an entity by its ID
        public T GetById(Guid id) => _dbSet.Find(id);

        // Get all entities
        public IEnumerable<T> GetAll() => _dbSet.ToList();

        // Find entities matching a predicate
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

        // Count entities matching a predicate
        public int Count(Expression<Func<T, bool>> predicate) => _dbSet.Count(predicate);

        // Async Methods
        public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).AsNoTracking().ToListAsync();

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) => await _dbSet.CountAsync(predicate);

        // Queryable interface for advanced queries
        public IQueryable<T> Query() => _dbSet.AsQueryable();

        // Additional Utilities
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);


        public IEnumerable<T> FindWithSpecification(Specification<T> specification)
        {
            return specification.Apply(_dbSet.AsQueryable()).ToList();
        }

        public async Task<IEnumerable<T>> FindWithSpecificationAsync(Specification<T> specification)
        {
            return await specification.Apply(_dbSet.AsQueryable()).ToListAsync();
        }


        public async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
        {
            return await _dbSet
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
