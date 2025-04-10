
using System.Threading.Tasks;
using EstateMapperLibrary;
using Newtonsoft.Json;
using RestSharp;

namespace EstateMapperClient.Services
{
    public class HttpRestClient
    {
        private readonly string _apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this._apiUrl = apiUrl;
            client = new RestClient();

        }
        public async Task<ApiResponse> ExcuExecuteAysnc(BaseRequest baseRequest)
        {
            var request = new RestRequest(_apiUrl + baseRequest.Route,baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse>(response.Content);

        }
        public async Task<ApiResponse<T>> ExcuExecuteAysnc<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(_apiUrl + baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        }

    }
}
