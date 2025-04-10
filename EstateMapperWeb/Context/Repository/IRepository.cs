using System.Linq.Expressions;
using EstateMapperLibrary.Models;

namespace EstateMapperWeb.Context.Repository
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        // 增
        Task<TEntity> CreateAsync(TEntity entity);

        // 删
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TKey id);

        // 改
        Task<TEntity> UpdateAsync(TEntity entity);

        // 查
        Task<TEntity> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();

        // 分页
        
    }
}
