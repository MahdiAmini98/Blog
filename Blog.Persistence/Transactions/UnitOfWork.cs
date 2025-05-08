using Blog.Domain.Interfaces;
using Blog.Persistence.Contexts;
using Blog.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Persistence.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _currentTransaction;

        public UnitOfWork(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _context = dbContextFactory.CreateDbContext();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        // Commit synchronous
        public void Commit()
        {
            _context.SaveChanges();
        }

        // Commit asynchronous
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Rollback: Typically EF Core handles this automatically if Commit is not called
        public void Rollback()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Rollback();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        // Rollback asynchronous
        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        // Begin transaction synchronous
        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = _context.Database.BeginTransaction();
        }

        // Begin transaction asynchronous
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        // Commit transaction synchronous
        public void CommitTransaction()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction to commit.");
            }

            _currentTransaction.Commit();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        // Commit transaction asynchronous
        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction to commit.");
            }

            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        // Rollback transaction synchronous
        public void RollbackTransaction()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction to rollback.");
            }

            _currentTransaction.Rollback();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        // Rollback transaction asynchronous
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction to rollback.");
            }

            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        // Dispose context
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
