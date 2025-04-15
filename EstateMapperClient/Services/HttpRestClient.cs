using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EstateMapperLibrary;
using Newtonsoft.Json;
using Prism.Ioc;
using RestSharp;

namespace EstateMapperClient.Services
{
    public class HttpRestClient
    {
        private readonly string _apiUrl;
        private readonly IContainerProvider containerProvider;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this._apiUrl = apiUrl;
            client = new RestClient();
        }

        public async Task<ApiResponse> ExcuExecuteAysnc(BaseRequest baseRequest)
        {
            var request = new RestRequest(_apiUrl + baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            var token = TokenStorage.ReadToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }
            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Token 失效时的处理（如跳转登录）
                TokenStorage.DeleteToken();
            }
            return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
        }

        public async Task<ApiResponse<T>> ExcuExecuteAysnc<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(_apiUrl + baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            var token = TokenStorage.ReadToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }
            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Token 失效时的处理（如跳转登录）
                TokenStorage.DeleteToken();
            }
            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        }
    }
}
