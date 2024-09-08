# StockManagementAPI

This is a simple yet robust Stock Management API developed using .NET Core. The API provides endpoints for managing products, including creating products, updating prices with a delay, listing products with filtering, and sending email notifications upon certain events.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Endpoints](#endpoints)
- [Architecture](#architecture)
- [Future Improvements](#future-improvements)
- [Contributing](#contributing)

## Features
- **Product Management:** Create, update, and list products with stock and price information.
- **Price Update with Delay:** Prices can be updated with a delay, simulating a scheduled update.
- **Filtering:** Products can be listed with filtering options for price and stock.
- **Email Notifications:** Sends email notifications when certain actions (like price updates) occur.
- **FluentValidation:** Ensures input data is valid, such as checking that product names do not contain numbers and that stock and price values are positive.

## Technologies Used
- **.NET Core 6/7/8:** The backbone of the application.
- **Entity Framework Core:** For database interactions using the Code First approach.
- **FluentValidation:** For input validation.
- **SMTP (System.Net.Mail):** For sending email notifications.
- **Swagger:** For API documentation and testing.
- **Hangfire:** For background Scheduled Jobs.
- **AutoMapper:** For mapping cross models

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server or any compatible database.
- A tool to test APIs like Postman or Curl.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/StockManagementAPI.git

2. Navigate to the project directory:
   ```bash
   cd StockManagementAPI

3. Restore the dependencies:
   ```bash
    dotnet restore

4. Apply migrations and update the database:
   ```bash
    dotnet ef database update

5. Run the application
   ```bash
    dotnet run

6. Open your browser and navigate to https://localhost:5001/swagger to explore the API using Swagger UI.

## Configuration
### Database Configuration
  The connection string for the database is configured in the appsettings.json file:
  ```json 
  "ConnectionStrings": {
   "DefaultConnection": "Server=localhost,15432;Database=StockManagementDb;User Id=sa;Password=AdventurePass*123;Trusted_Connection=False;MultipleActiveResultSets=true"
  }
  ```

## Email Configuration
Email settings are also configured in appsettings.json:
``` json
"EmailSettings": {
  "SmtpServer": "smtp.example.com",
  "SmtpPort": 587,
  "SenderName": "Your Name",
  "SenderEmail": "your-email@example.com",
  "Username": "your-smtp-username",
  "Password": "your-smtp-password"
}
```

## Endpoints
### Endpoints
* Create a Product
* POST /api/products
* Request Body:
``` json
{
  "name": "Sample Product",
  "stockQuantity": 50,
  "price": 19.99
}
```

### Update Product Price with Delay
* PUT /api/products/{id}/price
* Request Body:
``` json
{
  "ProductID": 1
  "newPrice": 25.00
}
```

### List Products with Filtering
* GET /api/products?minPrice=10&maxPrice=50&minStock=10
### Get Product by ID
* GET /api/products/{id}
## Architecture
* CCC Layer: Contains the entities, models, dtos and Mapping Classes. Or holds utils and helper classes.
* BLL Layer: Handles business logic operations and external services like email. It uses Database services with DI interface.
* DAL Layer: (DataBase Access Layer) This layer implements the Repository Pattern to access the database. You can use Entity Framework or any alternative to Entity Framework.
* API Layer: Exposes the application functionality through HTTP endpoints. It handles incoming requests, delegates them to the appropriate services, and returns responses.

## Reflection and Extension-Based Service Registration
> In this project, we use reflection to dynamically discover and register services from different assemblies. Specifically, we look for assemblies whose names start with "StockManagement" and only consider those for service registration. This allows for a modular architecture where each component can independently register its own services using a naming convention.

> We achieve this by scanning for an extension method named RegisterStockManagementServices in each assembly that extends IServiceCollection. This method is responsible for registering all the services in the respective assembly.

### Here’s how it works:

* Selective Assembly Scanning: We don't scan all assemblies; instead, we limit the scan to only those whose names start with "StockManagement." This ensures that we focus on relevant modules and avoid unnecessary reflection overhead.

* Dynamic Service Registration: Once we find an assembly, we use reflection to look for an extension method named RegisterStockManagementServices. If it exists, we invoke it to register the services for that module.

This approach promotes scalability and flexibility by allowing each module to self-register its dependencies, while keeping the registration process dynamic and based on naming conventions.

## For Example:
> Location: StockManagement.API/Helper/ServiceRegistrator.cs
```csharp
public static void AddReferencedProjectServices(this IServiceCollection services, Assembly[] assemblies, IConfiguration configurationManager) {
    //  We are scanning all the referenced assemblies.
    foreach (var assembly in assemblies) {
        // We are retrieving all the types in each assembly.
        var types = assembly.GetTypes();

        // We are finding the static methods that take an 'IServiceCollection' parameter.
        var serviceRegistrationMethods = types
        .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        .Where(m => m.GetParameters().Any(p => p.ParameterType == typeof(IServiceCollection)));

        // Invoke methods
        foreach (var method in serviceRegistrationMethods) {
            if(method.Name != "RegisterStockManagementServices") {
                continue;
            }
            // Ensure that the first parameter of the method is 'IServiceCollection'.
            var parameters = method.GetParameters();
            if(parameters.Length == 0) {
                return;
            }
            if(parameters.Length == 1) {
                if (parameters[0].ParameterType == typeof(IServiceCollection)) {
                    method.Invoke(null, new object[] { services });
                }
            }
            if (parameters.Length == 2) {
                if (parameters[0].ParameterType == typeof(IServiceCollection) &&
                    parameters[1].ParameterType == typeof(IConfiguration)) {
                    method.Invoke(null, new object[] { services, configurationManager });
                }
            }
        }
    }
}
```
## Background Task Processing with Hangfire

In this project, we use **Hangfire** to handle background tasks, such as delayed price updates and email notifications. Instead of blocking the main thread with `Task.Delay`, we utilize Hangfire to schedule and manage background jobs efficiently.

### What is Hangfire?

**Hangfire** is an open-source library that helps manage background tasks in .NET applications. It allows us to create, process, and monitor background jobs without impacting the main application performance. Hangfire provides a web-based dashboard to track all jobs and their status.

### Why Hangfire?

In our project, we needed to:
1. **Schedule Price Updates:** After a certain delay (e.g., 5 minutes), update the product price in the database.
2. **Send Email Notifications:** Notify users once the price update has been completed.

Hangfire allows us to schedule these tasks asynchronously, ensuring that the application can immediately respond to requests without being blocked by long-running operations.

### Key Features:

- **Background Jobs:** Jobs that can be executed asynchronously and are not dependent on the HTTP request-response lifecycle.
- **Scheduled Jobs:** Jobs that are scheduled to run at a specific time or after a defined delay (e.g., updating product prices after 5 minutes).
- **Retries:** If a job fails (due to network issues or other reasons), Hangfire automatically retries the job.
- **Dashboard:** Hangfire comes with a built-in dashboard to monitor job statuses, completion, and failures.

### How Hangfire Is Used in This Project

#### 1. Adding Hangfire to the Project

We added Hangfire to the project via NuGet packages:

```bash
dotnet add package Hangfire
dotnet add package Hangfire.EntityFrameworkCore
dotnet add package Hangfire.AspNetCore
```
We then configured Hangfire in Program.cs to use SQL Server for storing job details:

## Contributing
    Contributions are welcome! Please submit a pull request or open an issue to suggest improvements or report bugs.