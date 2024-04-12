using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ParsingProject;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.MappingProfiles;
using ParsingProject.BLL.Services;
using ParsingProject.DAL.Context;
using ParsingProject.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IParsingService, ParsingService>();
builder.Services.AddSingleton<ParsingUpdateHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<ParsingUpdateHostedService>());

builder.Services.AddDbContext<ParsingProjectContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(TestProfile)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseParsingProjectContext();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();