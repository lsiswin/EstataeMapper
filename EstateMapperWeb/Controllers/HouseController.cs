using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateMapperWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService service;

        public HouseController(IHouseService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<HouseDto>> GetHouseById(int id) =>
            await service.GetByIdAsync(id);

        [HttpGet]
        public async Task<ApiResponse<PagedResult<HouseDto>>> GetPagedAsync(
            [FromBody] HousePagedRequest request
        ) => await service.GetPagedAsync(request);

        [HttpPost]
        public async Task<ApiResponse<HouseDto>> CreateAsync([FromBody]HouseDto entity) =>
            await service.CreateAsync(entity);
        [HttpPut]
        public async Task<ApiResponse<HouseDto>> UpdateAsync([FromBody] HouseDto entity) => 
            await service.UpdateAsync(entity);

        [HttpDelete]
        public async Task<ApiResponse> DeleteHouse([FromBody] HouseDto entity) => 
            await service.DeleteAsync(entity);

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteHouse(int id) =>
            await service.DeleteAsync(id);
        [HttpGet]
        public async Task<ApiResponse<List<Region>>> GetRegionsAsync() => await service.GetRegionsAsync();

    }
}
