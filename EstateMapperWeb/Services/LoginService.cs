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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginService(
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration
        )
        {
            this.mapper = mapper;
            this.roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponse>> Login(UserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
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
            var token = await GenerateJwtToken(user);
            return new ApiResponse<LoginResponse>(
                ResultStatus.OK,
                new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(1) },
                "登录成功"
            );
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            var roles = await _userManager.GetRolesAsync(user); // 获取用户角色
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // 添加角色声明
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = jwtSettings["validAudience"],
                Issuer = jwtSettings["validIssuer"],
                Subject = new ClaimsIdentity(claims),
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
            var alreadyExists = await roleManager.RoleExistsAsync("Admin");
            if (!alreadyExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            var result = await _userManager.CreateAsync(user, dto.Password);
            await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                return new ApiResponse<LoginResponse>(ResultStatus.BADREQUEST, null, "注册失败");
            }
            // 生成 Token 逻辑
            var token = await GenerateJwtToken(user);
            return new ApiResponse<LoginResponse>(
                ResultStatus.OK,
                new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(1) },
                "注册成功"
            );
        }
    }
}
