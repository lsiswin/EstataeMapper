using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using RestSharp;

namespace EstateMapperClient.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> DeleteAsync(TEntity entity);
        Task<ApiResponse<TEntity>> GetFirstOfDefaulAsync(int id);
        Task<ApiResponse<PagedResult<TEntity>>> GetPageAsync(PagedRequest parameter);
    }
}
