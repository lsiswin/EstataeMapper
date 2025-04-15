using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateMapperWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILoginService service;

        public AccountController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse<LoginResponse>> Register([FromBody] UserDto user) => await service.Register(user);

        [HttpPost]
        public async Task<ApiResponse<LoginResponse>> Login([FromBody] UserDto user) => await service.Login(user);
       
    }
}
