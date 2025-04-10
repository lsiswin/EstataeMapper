using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public class LayoutService : BaseService<LayoutDto>, ILayoutService
    {
        private readonly HttpRestClient client;

        public LayoutService(HttpRestClient client)
            : base("Layout", client)
        {
            this.client = client;
        }

        public async Task<ApiResponse<LayoutDto>> GetLayoutByHouseId(int houseId)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/Layout/GetLayoutByHouseId/{houseId}";
            return await client.ExcuExecuteAysnc<LayoutDto>(request);
        }
    }
}
