using AutoMapper;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Context.Repository;

namespace EstateMapperWeb.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> DeleteAsync(TagDto entity)
        {
            await unitOfWork.Tags.DeleteAsync(mapper.Map<Tag>(entity));
            return new ApiResponse(ResultStatus.DELETED, "删除成功");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            await unitOfWork.Tags.DeleteAsync(id);
            return new ApiResponse(ResultStatus.DELETED, "删除成功");
        }

        public async Task<ApiResponse<TagDto>> GetByIdAsync(int id)
        {
            var tag = await unitOfWork.Tags.GetByIdAsync(id);
            if (tag != null)
                return new ApiResponse<TagDto>(ResultStatus.OK, mapper.Map<TagDto>(tag), "请求成功");
            return new ApiResponse<TagDto>(ResultStatus.BADREQUEST, null, "请求失败");
        }

        public async Task<ApiResponse<TagDto>> GetByNameAsync(string name)
        {
            var tag = await unitOfWork.Tags.GetByNameAsync(name);
            if(tag!=null)
                return new ApiResponse<TagDto>(ResultStatus.OK, mapper.Map<TagDto>(tag), "请求成功");
            return new ApiResponse<TagDto>(ResultStatus.BADREQUEST,null, "请求失败");
        }
    }
}
