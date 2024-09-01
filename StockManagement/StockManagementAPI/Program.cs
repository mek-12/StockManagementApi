using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StockManagementAPI.API.Validations;
using StockManagementAPI.Core.Interfaces;
using StockManagementAPI.Core.Services;
using StockManagementAPI.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Entity Framework Core için DbContext'i kaydet
var services = builder.Services;
services.AddDbContext<StockManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IProductService, ProductService>();
services.AddScoped<IEmailService, EmailService>();

services.AddControllers();
services.AddFluentValidation(options => {
    options.RegisterValidatorsFromAssemblyContaining<ProductRequestValidator>();
});

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
