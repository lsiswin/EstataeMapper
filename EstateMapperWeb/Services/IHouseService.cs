using System.Linq.Expressions;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateMapperWeb.Services
{
    public interface IHouseService
    {
        Task<ApiResponse<HouseDto>> CreateAsync(HouseDto entity);

        // 删
        Task<ApiResponse> DeleteAsync(HouseDto entity);
        Task<ApiResponse> DeleteAsync(int id);

        // 改
        Task<ApiResponse<HouseDto>> UpdateAsync(HouseDto entity);

        // 查
        Task<ApiResponse<HouseDto>> GetByIdAsync(int id);

        // 分页
        Task<ApiResponse<PagedResult<HouseDto>>> GetPagedAsync(HousePagedRequest pagedRequest);

        Task<ApiResponse<List<Region>>> GetRegionsAsync();
    }
}
