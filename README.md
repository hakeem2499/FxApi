
# FxApi

## Overview

**FxApi** is a web API designed for managing stocks, portfolios, and user interactions in a financial application. This application leverages .NET Core with robust principles of API development, including authentication, data validation, and modular design. It also integrates financial data using third-party services to ensure a seamless and accurate user experience.

## Features

- **Stock Management**: CRUD operations for stocks, including fetching data from external financial services.
- **Portfolio Management**: Allows users to manage their stock portfolios, including adding and removing stocks.
- **Comment Functionality**: Users can post, view, update, and delete comments for stocks.
- **User Authentication**: Secure login and user management using ASP.NET Identity.
- **Query Support**: Flexible querying capabilities for fetching filtered data.
- **Integration with Financial APIs**: Supports external financial market data for accurate stock information.

## Technologies Used

- **Backend Framework**: ASP.NET Core Web API
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: ASP.NET Identity
- **Dependency Injection**: Built-in .NET Core DI container
- **Third-Party API**: Financial Modeling Prep (FMP) service for stock data

## Project Structure

```plaintext
FxApi/
├── Controllers/         # API controllers for handling HTTP requests
├── Data/                # Entity Framework Core database context
├── Dtos/                # Data transfer objects for input/output models
├── Extensions/          # Extension methods for utilities and helpers
├── Interfaces/          # Service and repository contracts
├── Mappers/             # Mapping logic between models and DTOs
├── Models/              # Database entities
├── Repositories/        # Business logic for data access
├── Helpers/             # Utility classes
└── Program.cs           # Entry point of the application
```

## Getting Started

### Prerequisites

1. [.NET SDK](https://dotnet.microsoft.com/download) (Version 6.0 or later recommended)
2. [SQL Server](https://www.microsoft.com/sql-server)
3. [Postman](https://www.postman.com/) or similar tool for API testing
4. Optional: [Docker](https://www.docker.com/) for containerized deployment

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/hakeem2499/FxApi.git
   cd FxApi
   ```

2. Set up the database:
   - Update the connection string in `appsettings.json` to match your SQL Server configuration.
   - Run the following command to apply migrations:
     ```bash
     dotnet ef database update
     ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Test the API by navigating to:
   - Swagger UI: `https://localhost:5001/swagger`
   - Alternatively, use Postman for testing.

### API Endpoints

| Endpoint                          | Method | Description                     |
|-----------------------------------|--------|---------------------------------|
| `/api/stock`                      | GET    | Retrieve all stocks            |
| `/api/stock/{id}`                 | GET    | Retrieve stock by ID           |
| `/api/portfolio`                  | GET    | Retrieve user portfolio         |
| `/api/portfolio/{symbol}`         | POST   | Add a stock to the portfolio   |
| `/api/comment`                    | POST   | Create a comment               |
| `/api/comment/{id}`               | DELETE | Delete a comment               |

## Contribution Guidelines

1. Fork the repository.
2. Create a new branch for your feature:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add feature name"
   ```
4. Push the branch and create a pull request:
   ```bash
   git push origin feature-name
   ```

## License

This project is licensed under the **MIT License**. See the [LICENSE](./LICENSE) file for details.

## Contact

For any questions or feedback, feel free to reach out to [Hakeem](emmanuelokore67@gmail.com).
