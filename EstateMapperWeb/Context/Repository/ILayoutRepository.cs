using EstateMapperLibrary.Models;

namespace EstateMapperWeb.Context.Repository
{
    public interface ILayoutRepository : IRepository<Layout, int>
    {
        Task<IEnumerable<Layout>> GetByHouseIdAsync(int houseId);
        Task BulkUpdateAsync(int houseId, IEnumerable<Layout> layouts);
        Task<Layout> FindAsync(Layout layout);
    }
}
