using EstateMapperLibrary;
using EstateMapperLibrary.Models;

namespace EstateMapperWeb.Services
{
    public interface ILoginService
    {
        Task<ApiResponse<LoginResponse>> Login(UserDto user);
        Task<ApiResponse<UserDto>> Register(UserDto user);
    }
}
