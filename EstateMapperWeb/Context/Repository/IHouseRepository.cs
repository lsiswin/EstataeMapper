using System.Linq.Expressions;
using EstateMapperLibrary.Models;

namespace EstateMapperWeb.Context.Repository
{
    public interface IHouseRepository : IRepository<House, int>
    {
        Task<PagedResult<House>> GetPagedAsync(PagedRequest pagedRequest);
        Task<PagedResult<House>> GetPagedAsync(
            Expression<Func<House, bool>> predicate,
            PagedRequest pagedRequest
        );
        Task<PagedResult<House>> GetPagedAsync(
            Expression<Func<House, bool>> predicate,
            Func<IQueryable<House>, IOrderedQueryable<House>> orderBy,
            PagedRequest pagedRequest,
            params Expression<Func<House, object>>[] includes
        );
        Task<PagedResult<House>> SearchPropertiesAsync(HousePagedRequest request);
        Task DeleteWithDependenciesAsync(int houseId);

        Task<List<Region>> GetRegionsAsync();
    }
}
