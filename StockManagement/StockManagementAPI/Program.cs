using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StockManagement.API.Validations;
using StockManagement.API.Application;
using StockManagement.API.Application.Entities;
using StockManagement.API.Application.Factories;
using StockManagement.API.Core.Interfaces;
using StockManagement.API.Core.Services;
using StockManagement.API.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString(Constants.DEFAULT_CONNECTION);

// Entity Framework Core için DbContext'i kaydet
var services = builder.Services;
services.AddDbContext<StockManagementDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddMemoryCache();
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IEmailService, EmailService>();
services.AddSingleton<IConnectionMultiplexer>(provider => {
    var cacheSettings = provider.GetRequiredService<IOptions<CacheSettings>>().Value;
    var configurationOptions = new ConfigurationOptions
    {
        EndPoints = { $"{cacheSettings.RedisConfiguration.Host}:{cacheSettings.RedisConfiguration.Port}" },
        Password = cacheSettings.RedisConfiguration.Password,
        DefaultDatabase = cacheSettings.RedisConfiguration.Database,
        AbortOnConnectFail = false
    };
    return ConnectionMultiplexer.Connect(configurationOptions);
});
// Burada ICacheService i inject ettim. Ve Requeired Service ile zorunlu
// kılınan class tan CreateCacheService metodu ile hangi cacheService i kullanacağını belirtiyoruz.
services.AddTransient<MemoryCacheService>();
services.AddTransient<RedisCacheService>();
services.AddScoped<ICacheService>(provider =>
    provider.GetRequiredService<CacheServiceFactory>().CreateCacheService());
services.AddControllers();
services.AddFluentValidation(options => {
    options.RegisterValidatorsFromAssemblyContaining<ProductRequestValidator>();
});
services.Configure<CacheSettings>(builder.Configuration.GetSection(Constants.CACHE_SETTINGS));

// After migration:
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
