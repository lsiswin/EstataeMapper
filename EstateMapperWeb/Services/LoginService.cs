using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Azure.Core;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Context.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EstateMapperWeb.Services
{
    public class LoginService : ILoginService
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginService(
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager
,
            IConfiguration configuration)
        {
            this.mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponse>> Login(UserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null) {
                return new ApiResponse<LoginResponse>(ResultStatus.BADREQUEST, null, "用户不存在");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: false
            );
            if (!result.Succeeded)
            {
                return new ApiResponse<LoginResponse>(ResultStatus.BADREQUEST, null, "登录失败");
            }
            // 生成 Token 逻辑
            var token = GenerateJwtToken(dto.UserName);
            return new ApiResponse<LoginResponse>(
                ResultStatus.OK,
                new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(1) },
                "登录成功"
            );
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ApiResponse<LoginResponse>> Register(UserDto dto)
        {
            var user = mapper.Map<User>(dto);
            //await _userManager.AddToRoleAsync(user,"User");
            var result = await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                return new ApiResponse<LoginResponse>(ResultStatus.BADREQUEST, null, "注册失败");
            }
            // 生成 Token 逻辑
            var token = GenerateJwtToken(dto.Email);
            return new ApiResponse<LoginResponse>(
                ResultStatus.OK,
                new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(1) },
                "注册成功"
            );
        }
    }
}
