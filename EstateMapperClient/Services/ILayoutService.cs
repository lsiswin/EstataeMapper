using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Services
{
    public interface ILayoutService:IBaseService<LayoutDto>
    {
        Task<ApiResponse<LayoutDto>> GetLayoutByHouseId(int houseId);
    }
}
