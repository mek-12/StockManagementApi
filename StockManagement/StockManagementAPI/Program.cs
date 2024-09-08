using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StockManagement.API.Validations;
using StockManagement.CCC;
using StockManagement.API.Helper;

var builder = WebApplication.CreateBuilder(args);

// Entity Framework Core için DbContext'i kaydet
var services = builder.Services;
IConfiguration configuration = builder.Configuration;
ServiceRegistrator.AddReferencedProjectServices(services, AppDomain.CurrentDomain.GetAssemblies(), configuration);

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
