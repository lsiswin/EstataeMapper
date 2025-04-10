using EstateMapperLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateMapperWeb.Context.Repository
{
    public class LayoutRepository : Repository<Layout, int>, ILayoutRepository
    {
        private DbSet<Layout> _layouts;

        public LayoutRepository(MyContext context)
            : base(context)
        {
            _layouts = context.Set<Layout>();
        }

        public async Task BulkUpdateAsync(int houseId, IEnumerable<Layout> layouts)
        {
            foreach (var layout in layouts)
            {
                layout.Id = houseId;
                await _layouts.AddAsync(layout);
            }
        }

        public async Task<Layout> FindAsync(Layout layout)
        {
            return await _layouts
                .Where(p => p.LayoutName == layout.LayoutName)
                .Where(p => p.HouseId == layout.HouseId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Layout>> GetByHouseIdAsync(int houseId)
        {
            return await _layouts.Where(x => x.HouseId == houseId).ToListAsync();
        }
    }
}
