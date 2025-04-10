using System.Diagnostics;
using System.Linq.Expressions;
using AutoMapper;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Context.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateMapperWeb.Services
{
    public class HouseService : IHouseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public HouseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<HouseDto>> CreateAsync(HouseDto dto)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var house = mapper.Map<House>(dto);
                await unitOfWork.Houses.CreateAsync(house);
                await unitOfWork.CommitAsync();
                var resultDto = mapper.Map<HouseDto>(house);
                return new ApiResponse<HouseDto>(ResultStatus.CREATED, resultDto, "创建成功");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<HouseDto>(
                    ResultStatus.ERROR,
                    null,
                    $"创建失败: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse> DeleteAsync(HouseDto entity)
        {
            await DeleteAsync(entity.Id);
            return new ApiResponse(ResultStatus.DELETED, "删除成功");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                // 执行级联删除
                await unitOfWork.Houses.DeleteWithDependenciesAsync(id);
                await unitOfWork.CommitAsync();

                return new ApiResponse(ResultStatus.DELETED, "删除成功");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse(ResultStatus.ERROR, $"删除失败: {ex.Message}");
            }
        }

        public async Task<ApiResponse<HouseDto>> GetByIdAsync(int id)
        {
            using var transaction = unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await unitOfWork.Houses.GetByIdAsync(id);

                return new ApiResponse<HouseDto>(
                    ResultStatus.OK,
                    mapper.Map<HouseDto>(result),
                    "请求成功"
                );
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<HouseDto>(
                    ResultStatus.ERROR,
                    null,
                    $"请求失败:{ex.Message}"
                );
                throw;
            }
        }

        public async Task<ApiResponse<PagedResult<HouseDto>>> GetPagedAsync(
            HousePagedRequest pagedRequest
        )
        {
            using var transaction = unitOfWork.BeginTransactionAsync();
            try
            {
                var houselist = await unitOfWork.Houses.SearchPropertiesAsync(pagedRequest);
                if(houselist == null)
                {
                    return new ApiResponse<PagedResult<HouseDto>>(
                        ResultStatus.NOTFOUND,
                        null,
                        "数据不存在"
                    );
                }
                PagedResult<HouseDto> pageList = new PagedResult<HouseDto>
                {
                    PageNumber = houselist.PageNumber,
                    PageSize = houselist.PageSize,
                    TotalCount = houselist.TotalCount,
                };
                foreach (var item in houselist.Items)
                {
                    var page = mapper.Map<HouseDto>(item);
                    pageList.Items.Add(page);
                }
                await unitOfWork.CommitAsync();
                return new ApiResponse<PagedResult<HouseDto>>(
                    ResultStatus.OK,
                    pageList,
                    "请求成功"
                );
                
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<PagedResult<HouseDto>>(
                    ResultStatus.ERROR,
                    null,
                    $"请求失败:{ex.Message}"
                );
                throw;
            }
        }

        public async Task<ApiResponse<List<Region>>> GetRegionsAsync()
        {
            try
            {
                var regions = await unitOfWork.Houses.GetRegionsAsync();
                return new ApiResponse<List<Region>>(
                    ResultStatus.OK,
                    regions,
                    "请求成功"
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Region>>(
                    ResultStatus.ERROR,
                    null,
                    $"请求失败:{ex.Message}"
                );
                throw;
            }
           
        }

        public async Task<ApiResponse<HouseDto>> UpdateAsync(HouseDto dto)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                // 获取包含完整关联的房屋实体
                var existingHouse = await unitOfWork.Houses.GetByIdAsync(dto.Id);

                if (existingHouse == null)
                    return new ApiResponse<HouseDto>(ResultStatus.NOTFOUND, null, "数据不存在");
                mapper.Map(dto, existingHouse);
                await unitOfWork.Houses.UpdateAsync(existingHouse);

                await unitOfWork.CommitAsync();
                return new ApiResponse<HouseDto>(
                    ResultStatus.OK,
                    mapper.Map<HouseDto>(existingHouse),
                    "更新成功"
                );
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<HouseDto>(
                    ResultStatus.ERROR,
                    null,
                    $"更新失败: {ex.Message}"
                );
            }
        }
    }
}
