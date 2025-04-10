using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public class HouseService : BaseService<HouseDto>, IHouseService
    {
        private readonly HttpRestClient client;

        public HouseService(HttpRestClient client) : base("House", client)
        {
            this.client = client;
        }

        public async Task<ApiResponse<List<Region>>> GetRegionsAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/house/GetRegions";
            return await client.ExcuExecuteAysnc<List<Region>>(request);
        }
    }
}
