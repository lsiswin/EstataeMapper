using EstateMapperLibrary.Models;
using EstateMapperLibrary;
using EstateMapperWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateMapperWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TagController:ControllerBase
    {
        private readonly ITagService service;

        public TagController(ITagService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<TagDto>> GetTagById(int id) =>
            await service.GetByIdAsync(id);

        [HttpGet]
        public async Task<ApiResponse<TagDto>> GetTagByName([FromQuery]string name) =>
            await service.GetByNameAsync(name);

        
        [HttpDelete]
        public async Task<ApiResponse> DeleteTag([FromBody] TagDto entity) =>
            await service.DeleteAsync(entity);

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteTag(int id) =>
            await service.DeleteAsync(id);

    }
}
