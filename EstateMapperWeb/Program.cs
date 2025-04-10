using System;
using EstateMapperWeb.Context;
using EstateMapperWeb.Context.Repository;
using EstateMapperWeb.Profiles;
using EstateMapperWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(option =>
{
    var server = builder.Configuration.GetConnectionString("Mysql");
    option.UseMySql(server, ServerVersion.AutoDetect(server), o => o.CommandTimeout(300));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));

// 2. ×¢²á·ºÐÍ²Ö¿â
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

// 3. ×¢²á¾ßÌå²Ö¿â
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ILayoutRepository, LayoutRepository>();

builder.Services.AddTransient<ILayoutService, LayoutService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IHouseService, HouseService>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.Run();
