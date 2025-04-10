using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public class TagService : BaseService<TagDto>, ITagService
    {
        private readonly HttpRestClient client;

        public TagService(HttpRestClient client) : base("Tag", client)
        {
            this.client = client;
        }

        public async Task<ApiResponse<TagDto>> GetTagByName(string name)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/Tag/GetTagByName";
            request.Parameter = name;
            return await client.ExcuExecuteAysnc<TagDto>(request);
        }
    }
}
