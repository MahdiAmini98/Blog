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
        private readonly AppDbContext _externalContext;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private bool UseExternalContext => _externalContext != null;

        // این سازنده فقط در داخل پروژه (برای UnitOfWork) قابل دسترسی است
        internal Repository(AppDbContext context)
        {
            _externalContext = context;
        }


        public Repository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }


        private async Task<TResult> ExecuteAsync<TResult>(Func<AppDbContext, Task<TResult>> operation)
        {
            if (UseExternalContext)
            {
                return await operation(_externalContext);
            }
            else
            {
                using var context = _contextFactory.CreateDbContext();
                return await operation(context);
            }
        }

        private TResult Execute<TResult>(Func<AppDbContext, TResult> operation)
        {
            if (UseExternalContext)
            {
                return operation(_externalContext);
            }
            else
            {
                using var context = _contextFactory.CreateDbContext();
                return operation(context);
            }
        }


        public void Add(T entity)
        {
            if (UseExternalContext)
            {
                _externalContext.Set<T>().Add(entity);
            }
            else
            {
                using var context = _contextFactory.CreateDbContext();
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public void Remove(T entity)
        {
            if (UseExternalContext)
            {
                _externalContext.Set<T>().Remove(entity);
            }
            else
            {
                using var context = _contextFactory.CreateDbContext();
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            if (UseExternalContext)
            {
                _externalContext.Set<T>().Update(entity);
            }
            else
            {
                using var context = _contextFactory.CreateDbContext();
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
        }

        public T GetById(Guid id)
        {
            return Execute(ctx => ctx.Set<T>().Find(id));
        }

        public IEnumerable<T> GetAll()
        {
            return Execute(ctx => ctx.Set<T>().ToList());
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Execute(ctx => ctx.Set<T>().Where(predicate).ToList());
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Execute(ctx => ctx.Set<T>().Count(predicate));
        }


        public async Task<T> GetByIdAsync(Guid id)
        {
            return await ExecuteAsync(async ctx => await ctx.Set<T>().FindAsync(id));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await ExecuteAsync(async ctx => await ctx.Set<T>().ToListAsync());
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            return await ExecuteAsync(async ctx =>
            {
                if (asNoTracking)
                    return await ctx.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
                else
                    return await ctx.Set<T>().Where(predicate).ToListAsync();
            });
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await ExecuteAsync(async ctx => await ctx.Set<T>().CountAsync(predicate));
        }


        public IQueryable<T> Query()
        {
            if (UseExternalContext)
            {
                return _externalContext.Set<T>().AsQueryable();
            }
            else
            {
                throw new InvalidOperationException(" Query نیاز به یک کانتکست خارجی دارد.");
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await ExecuteAsync(async ctx => await ctx.Set<T>().AnyAsync(predicate));
        }


        public IEnumerable<T> FindWithSpecification(Specification<T> specification)
        {
            return Execute(ctx =>
            {
                var query = ApplySpecification(ctx.Set<T>().AsQueryable(), specification);
                return query.ToList();
            });
        }

        public async Task<IEnumerable<T>> FindWithSpecificationAsync(Specification<T> specification)
        {
            return await ExecuteAsync(async ctx =>
            {
                var query = ApplySpecification(ctx.Set<T>().AsQueryable(), specification);
                return await query.ToListAsync();
            });
        }

        public async Task<int> CountWithSpecificationAsync(Specification<T> specification)
        {
            return await ExecuteAsync(async ctx =>
            {
                var query = ApplySpecification(ctx.Set<T>().AsQueryable(), specification);
                return await query.CountAsync();
            });
        }

        public async Task<IEnumerable<T>> FindWithSpecificationPagedAsync(Specification<T> specification, int page, int pageSize)
        {
            return await ExecuteAsync(async ctx =>
            {
                var query = ApplySpecification(ctx.Set<T>().AsQueryable(), specification);
                return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            });
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
        {
            return await ExecuteAsync(async ctx =>
            {
                return await ctx.Set<T>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            });
        }


        private IQueryable<T> ApplySpecification(IQueryable<T> query, Specification<T> specification)
        {
            query = specification.Apply(query);
            foreach (var includeExpression in specification.Includes)
            {
                query = query.Include(includeExpression);
            }
            return query;
        }
    }
}
