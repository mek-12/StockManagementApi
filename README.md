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
* Core Layer: Contains the entities and interfaces. This layer defines the business models and contracts (interfaces) that the application uses.
* Infrastructure Layer: Handles database operations and external services like email. It uses Entity Framework Core for database interactions.
* API Layer: Exposes the application functionality through HTTP endpoints. It handles incoming requests, delegates them to the appropriate services, and returns responses.

## Contributing
    Contributions are welcome! Please submit a pull request or open an issue to suggest improvements or report bugs.