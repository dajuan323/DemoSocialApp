# DemoSocial Backend Application

DemoSocial is a backend application built using .NET that follows Clean Architecture principles with CQRS (Command Query Responsibility Segregation) pattern.

## Features

- **Clean Architecture:** Organized into distinct layers with clear separation of concerns, promoting maintainability and testability.
  
- **CQRS (Command Query Responsibility Segregation):** Separates read and write operations, improving scalability, performance, and simplifying the codebase.

- **Domain-driven Design (DDD):** Focuses on the core domain logic, ensuring business rules are encapsulated within the domain models.

- **MediatR:** Implements Mediator pattern to decouple message senders and receivers, facilitating the implementation of CQRS.

- **Entity Framework Core:** ORM used for data persistence, ensuring efficient database interactions and migrations.

- **Swagger/OpenAPI:** Integrated Swagger for API documentation, making it easy to explore and test API endpoints.

## Technologies Used

- **.NET Core**: Framework for building cross-platform applications.
  
- **C#**: Primary programming language for implementing backend logic.

- **ASP.NET Core**: Framework for building APIs and web applications.

- **Entity Framework Core**: ORM for database operations.

- **MediatR**: Lightweight library for implementing Mediator pattern.

## Project Structure

The application is structured according to Clean Architecture principles:

- **Domain Layer**: Contains entities, value objects, domain services, and domain events.
  
- **Application Layer**: Implements use cases and business logic using CQRS pattern.
  
- **Infrastructure Layer**: Includes data access, external services, and implementation of infrastructure concerns.
  
- **Presentation Layer**: Houses API controllers and DTOs for handling HTTP requests and responses.

## Getting Started

To run the DemoSocial backend locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your/repository.git
   cd DemoSocial
