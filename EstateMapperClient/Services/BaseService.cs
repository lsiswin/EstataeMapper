using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly string serviceName;
        private readonly HttpRestClient client;

        public BaseService(string serviceName,HttpRestClient client) {
            this.serviceName = serviceName;
            this.client = client;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Create";
            request.Parameter = entity;
            return await client.ExcuExecuteAysnc<TEntity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"api/{serviceName}/Delete{serviceName}/{id}";
            return await client.ExcuExecuteAysnc(request);
        }

        public async Task<ApiResponse> DeleteAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"api/{serviceName}/Delete{serviceName}/";
            request.Parameter = entity;
            return await client.ExcuExecuteAysnc(request);
        }

        public async Task<ApiResponse<PagedResult<TEntity>>> GetPageAsync(PagedRequest parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetPaged";
            request.Parameter = parameter;
            return await client.ExcuExecuteAysnc<PagedResult<TEntity>>(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaulAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/Get{serviceName}ById/{id}";
            return await client.ExcuExecuteAysnc<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Update";
            request.Parameter = entity;
            return await client.ExcuExecuteAysnc<TEntity>(request);
        }
    }
}
