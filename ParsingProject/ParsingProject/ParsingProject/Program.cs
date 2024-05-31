using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ParsingProject;
using ParsingProject.BackgroundServices;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.MappingProfiles;
using ParsingProject.BLL.Services;
using ParsingProject.DAL.Context;
using ParsingProject.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IChannelParsingService, ChannelParsingService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<ChannelService>();
builder.Services.AddScoped<DBRepository>();
builder.Services.AddTransient<DataSeeder>();

builder.Services.AddSingleton<ParsingUpdateHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<ParsingUpdateHostedService>());

builder.Services.AddSingleton<WTelegramService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<WTelegramService>());

builder.Services.AddDbContext<ParsingProjectContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLowerCaseNamingConvention();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper();
builder.Services.AddValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseParsingProjectContext();

app.UseCors(opt => opt
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using var scope = scopedFactory?.CreateScope();
    var service = scope?.ServiceProvider.GetService<DataSeeder>();
    service?.Seed();
}

SeedData(app);

app.Run();