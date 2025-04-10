
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace EstateMapperWeb.Context.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _context;
        private IDbContextTransaction _currentTransaction;
        private bool _disposed;

        public UnitOfWork(MyContext context)
        {
            _context = context;
            Houses = new HouseRepository(context);
            Tags = new TagRepository(context);
            Layouts = new LayoutRepository(context);
        }

        public IHouseRepository Houses { get; }
        public ITagRepository Tags { get; }
        public ILayoutRepository Layouts { get; }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("已经有进行中的事务");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
            return _currentTransaction;
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                }
                else
                {
                    // 回退未提交的更改
                    foreach (var entry in _context.ChangeTracker.Entries()
                        .Where(e => e.State != EntityState.Unchanged))
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                            case EntityState.Deleted:
                                await entry.ReloadAsync();
                                break;
                        }
                    }
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
