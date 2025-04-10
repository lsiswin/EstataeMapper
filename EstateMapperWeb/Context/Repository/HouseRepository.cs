using System.Linq.Expressions;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EstateMapperWeb.Context.Repository
{
    public class HouseRepository : Repository<House, int>, IHouseRepository
    {
        private readonly MyContext context;
        private readonly ILogger<HouseService> logger;
        private readonly DbSet<House> dbSet;

        public HouseRepository(MyContext context)
            : base(context)
        {
            this.context = context;
            this.dbSet = context.Set<House>();
        }

        /// <summary>
        /// 基础分页
        /// </summary>
        /// <param name="pagedRequest"></param>
        /// <returns></returns>
        public async Task<PagedResult<House>> GetPagedAsync(PagedRequest pagedRequest)
        {
            return await GetPagedAsync(dbSet.AsQueryable(), pagedRequest);
        }

        /// <summary>
        /// 条件分页
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pagedRequest"></param>
        /// <returns></returns>
        public async Task<PagedResult<House>> GetPagedAsync(
            Expression<Func<House, bool>> predicate,
            PagedRequest pagedRequest
        )
        {
            var query = dbSet.Where(predicate);
            return await GetPagedAsync(query, pagedRequest);
        }

        /// <summary>
        /// 高级分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pagedRequest">分页数</param>
        /// <param name="includes">导航属性</param>
        /// <returns></returns>
        public async Task<PagedResult<House>> GetPagedAsync(
            Expression<Func<House, bool>> predicate,
            Func<IQueryable<House>, IOrderedQueryable<House>> orderBy,
            PagedRequest pagedRequest,
            params Expression<Func<House, object>>[] includes
        )
        {
            var query = dbSet.AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await GetPagedAsync(query, pagedRequest);
        }

        /// <summary>
        /// 分页核心方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagedRequest"></param>
        /// <returns></returns>
        protected async Task<PagedResult<House>> GetPagedAsync(
            IQueryable<House> query,
            PagedRequest pagedRequest
        )
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize)
                .ToListAsync();

            return new PagedResult<House>
            {
                PageNumber = pagedRequest.PageNumber,
                PageSize = pagedRequest.PageSize,
                TotalCount = totalCount,
                Items = items,
            };
        }

        public async Task<PagedResult<House>> SearchPropertiesAsync(HousePagedRequest request)
        {
            
            try
            {
                var query = context
                .House.AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.Tags)
                .Include(p => p.Layouts)
                .AsQueryable();
                if (!string.IsNullOrEmpty(request.Search))
                {
                    query = query.Where(p => p.Name.Contains(request.Search));
                }
                if (request.Price.HasValue)
                {
                    query = query.Where(p => p.Price < request.Price);
                }
                if (request.RegionId.HasValue)
                {
                    query = query.Where(p => p.SubRegionId == request.RegionId);
                }
                return await GetPagedAsync(query, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task DeleteWithDependenciesAsync(int houseId)
        {
            // 显式加载所有关联数据
            var house = await context
                .House.Include(h => h.Layouts)
                .Include(h => h.Tags)
                .FirstOrDefaultAsync(h => h.Id == houseId);

            if (house != null)
            {
                // 先清空Tags关联（如果不需要自动处理）
                house.Tags.Clear();

                context.House.Remove(house);
            }
        }

        public async Task<List<Region>> GetRegionsAsync()
        {
            return await context.Region.AsNoTracking().Include(r => r.SubRegions).ToListAsync();
        }
    }
}
