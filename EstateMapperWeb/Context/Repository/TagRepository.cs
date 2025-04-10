using System.Linq.Expressions;
using EstateMapperLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateMapperWeb.Context.Repository
{
    public class TagRepository : Repository<Tag, int>, ITagRepository
    {
        private DbSet<Tag> _dbSet;

        public TagRepository(MyContext context)
            : base(context)
        {
            _dbSet = context.Set<Tag>();
        }

        public async Task<Tag> GetByNameAsync(string tagName)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TagName == tagName);
        }
    }
}
