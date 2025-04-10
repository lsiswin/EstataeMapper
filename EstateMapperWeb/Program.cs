using System;
using System.Text;
using EstateMapperLibrary.Models;
using EstateMapperWeb.Context;
using EstateMapperWeb.Context.Repository;
using EstateMapperWeb.Profiles;
using EstateMapperWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    //var server = builder.Configuration.GetConnectionString("Mysql");
    //option.UseMySql(server, ServerVersion.AutoDetect(server));
});

builder
    .Services.AddIdentity<User, IdentityRole>(
        option => {
            option.Password.RequiredLength = 6;
            option.Password.RequireDigit = false;
            option.Password.RequireLowercase = false;
            option.Password.RequireUppercase = false;
            option.Password.RequireNonAlphanumeric = false;
        }
    )
    .AddEntityFrameworkStores<MyContext>()
    .AddDefaultTokenProviders();


// ÅäÖÃJWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["secretKey"])
            ),
        };
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthorization();

// 2. ×¢²á·ºÐÍ²Ö¿â
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

// 3. ×¢²á¾ßÌå²Ö¿â
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ILayoutRepository, LayoutRepository>();

builder.Services.AddTransient<ILayoutService, LayoutService>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IHouseService, HouseService>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.Run();
