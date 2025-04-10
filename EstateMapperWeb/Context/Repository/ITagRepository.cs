using EstateMapperLibrary.Models;

namespace EstateMapperWeb.Context.Repository
{
    public interface ITagRepository:IRepository<Tag,int>
    {
        Task<Tag> GetByNameAsync(string tagName);
    }
}
