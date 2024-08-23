# Using Clean Architecture

- SharedKernel project with common Domain-Driven Design abstractions.
- Domain layer with sample entities.
- Application layer with abstractions for:
  - CQRS
  - Cross-cutting concerns (logging, validation)
- Infrastructure layer with:
  - Authentication
  - Permission authorization
  - EF Core, MongoDb
  - Serilog