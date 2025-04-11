using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient httpClient;

        public LoginService(HttpRestClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/Login/Login";
            request.Parameter = user;
            return await httpClient.ExcuExecuteAysnc<LoginResponse>(request);
        }

        public async Task<ApiResponse<UserDto>> RegisterAsync(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/Login/Register";
            request.Parameter = user;
            return await httpClient.ExcuExecuteAysnc<UserDto>(request);
        }
    }
}
