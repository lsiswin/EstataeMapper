using Microsoft.EntityFrameworkCore.Storage;

namespace EstateMapperWeb.Context.Repository
{
    public interface IUnitOfWork:IDisposable
    {
        IHouseRepository Houses { get; }
        ITagRepository Tags { get; }
        ILayoutRepository Layouts { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();
    }
}
