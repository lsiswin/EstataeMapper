using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateMapperWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController:Controller
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse<UserDto>> Regist([FromBody] UserDto user) => await service.Register(user);

        [HttpPost]
        public async Task<ApiResponse<LoginResponse>> Login([FromBody] UserDto user) => await service.Login(user);
       
    }
}
