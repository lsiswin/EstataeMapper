using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public interface ITagService:IBaseService<TagDto>
    {
        Task<ApiResponse<TagDto>> GetTagByName(string name);
    }
}
