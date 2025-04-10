using EstateMapperLibrary.Models;
using EstateMapperLibrary;

namespace EstateMapperWeb.Services
{
    public interface ITagService
    {

        // 删
        Task<ApiResponse> DeleteAsync(TagDto entity);
        Task<ApiResponse> DeleteAsync(int id);


        // 查
        Task<ApiResponse<TagDto>> GetByIdAsync(int id);
        Task<ApiResponse<TagDto>> GetByNameAsync(string name);

    }
}