using EstateMapperLibrary.Models;
using EstateMapperLibrary;

namespace EstateMapperWeb.Services
{
    public interface ILayoutService
    {

        // 删
        Task<ApiResponse> DeleteAsync(LayoutDto entity);
        Task<ApiResponse> DeleteAsync(int id);

        // 改
        Task<ApiResponse<LayoutDto>> UpdateAsync(LayoutDto entity);

        // 查
        Task<ApiResponse<LayoutDto>> GetByIdAsync(int id);

        // 分页
        Task<ApiResponse<IEnumerable<LayoutDto>>> GetByHouseIdAsync(int houseId);
    }
}
