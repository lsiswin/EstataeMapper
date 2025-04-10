using System.Linq.Expressions;
using EstateMapperLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateMapperWeb.Context.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly MyContext context;
        private readonly DbSet<TEntity> dbSet;

        public Repository(MyContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }
        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TEntity entity)
        {
            dbSet.Remove(entity);
        }
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if(entity != null)
            {
                await DeleteAsync(entity);
            }
            
        }
        /// <summary>
        /// 获取全部实体
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        /// <summary>
        /// 根据ID查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(TKey id)
        {
           return await dbSet.FindAsync(id);
           
        }
        
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            return entity;
        }
        
    }
}
