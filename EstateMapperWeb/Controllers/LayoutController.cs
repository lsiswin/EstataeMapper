using EstateMapperLibrary.Models;
using EstateMapperLibrary;
using EstateMapperWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstateMapperWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LayoutController : ControllerBase
    {
        private readonly ILayoutService service;

        public LayoutController(ILayoutService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<LayoutDto>> GetLayoutById(int id) =>
            await service.GetByIdAsync(id);

        [HttpGet("{houseId}")]
        public async Task<ApiResponse<IEnumerable<LayoutDto>>> GetLayoutByHouseIdAsync(
            int houseId
        ) => await service.GetByHouseIdAsync(houseId);

        [HttpPut]
        public async Task<ApiResponse<LayoutDto>> UpdateAsync([FromBody] LayoutDto entity) =>
            await service.UpdateAsync(entity);

        [HttpDelete]
        public async Task<ApiResponse> DeleteLayout([FromBody] LayoutDto entity) =>
            await service.DeleteAsync(entity);

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteLayout(int id) =>
            await service.DeleteAsync(id);


    }
}
