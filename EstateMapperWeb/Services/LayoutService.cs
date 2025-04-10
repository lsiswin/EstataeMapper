using System.Collections;
using AutoMapper;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Context.Repository;

namespace EstateMapperWeb.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public LayoutService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> DeleteAsync(LayoutDto entity)
        {
            await unitOfWork.Layouts.DeleteAsync(mapper.Map<Layout>(entity));
            return new ApiResponse(ResultStatus.DELETED, "删除成功");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            await unitOfWork.Layouts.DeleteAsync(id);
            return new ApiResponse(ResultStatus.DELETED, "删除成功");
        }

        public async Task<ApiResponse<LayoutDto>> GetByIdAsync(int id)
        {
            var layout = await unitOfWork.Layouts.GetByIdAsync(id);
            if(layout!=null)
                return new ApiResponse<LayoutDto>(ResultStatus.OK,mapper.Map<LayoutDto>(layout),"请求成功");
            return new ApiResponse<LayoutDto>(ResultStatus.BADREQUEST, null, "请求失败");
        }

        public async Task<ApiResponse<IEnumerable<LayoutDto>>> GetByHouseIdAsync(int houseId)
        {
            var list = await unitOfWork.Layouts.GetByHouseIdAsync(houseId);
            if (list!=null)
            {
                List<LayoutDto> layouts = new List<LayoutDto>();
                foreach (var item in list)
                {
                    layouts.Add(mapper.Map<LayoutDto>(item));
                }
                return new ApiResponse<IEnumerable<LayoutDto>>(ResultStatus.OK, layouts, "请求成功");
            }
            return new ApiResponse<IEnumerable<LayoutDto>>(ResultStatus.BADREQUEST, null, "请求失败");
        }

        public async Task<ApiResponse<LayoutDto>> UpdateAsync(LayoutDto entity)
        {
            var layout = await unitOfWork.Layouts.UpdateAsync(mapper.Map<Layout>( entity));
            if (layout != null)
                return new ApiResponse<LayoutDto>(ResultStatus.OK, mapper.Map<LayoutDto>(layout), "更新成功");
            return new ApiResponse<LayoutDto>(ResultStatus.BADREQUEST, null, "更新失败");

        }
    }
}
