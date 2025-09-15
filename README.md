# MyAztendaBackend

MyAztendaBackend demonstrates a wide range of **classic GoF** and **modern architectural patterns**.  
Each item below links to the actual implementation in the codebase.

---

## Design Patterns Used

### GoF / Object-Oriented Patterns
- **[Repository Pattern](./Shared/Repositories/Abstractions/IRepository.cs)** – generic `IRepository<T>` abstraction with concrete `Repository<T>` to encapsulate persistence operations
- **[Factory](./Shared/Filtering/ParserFactory.cs)** – `ParserFactory.Get<TEntity>(mode)` decides at runtime which parser implementation to return
- **[Strategy](./Shared/Filtering/IFilterParser.cs)** – `IFilterParser<TEntity>` with `AdvancedFilterParser<TEntity>` vs. `SimpleFilterParser<TEntity>`
- **[Decorator](./Shared/Cqrs/Decorators/LoggingCommandDecorator.cs)** – cross-cutting concerns (logging, validation) wrapped around handlers
- **[Command](./Shared/Cqrs/Abstractions/ICommandHandler.cs)** – requests encapsulated as `ICommand` objects with dedicated handlers
- **[Chain of Responsibility](./Shared/Cqrs/DependencyInjection/AddCqrs.cs)** – decorators registered in sequence to process/delegate requests
- **[Template Method](./Shared/Cqrs/Bases/PagedSearchHandler.cs)** – base classes outline algorithms, with steps overridden in subclasses

### CQRS & Mediator
- **[CQRS](./Shared/Cqrs/Bases/CreateEntityHandler.cs)** – `CreateEntityHandler` defines command workflow with overridable steps
- **[Mediator](./Shared/Cqrs/MiniMediator.cs)** – routes commands/queries to handlers via DI
- **[Service Locator](./Shared/Cqrs/MiniMediator.cs)** – dynamic resolution of handlers from the DI container

### DTO & Middleware
- **[DTO](./EmployeeService/DTOs/EmployeeDto.cs)** – simple record types carry data between layers
- **[Middleware](./Shared/Web/Middleware/GlobalExceptionMiddleware.cs)** – intercepts HTTP requests to handle errors globally

### Architectural Patterns
- **[API Gateway](./ApiGatewayService/ocelot.json)** – Ocelot routes `/customer-gate` endpoints to the customer service

---

## TODO / Next Steps

- [ ] Change naming of services to proper generic (e.g., `EmployeeService` → `HumanResources` where applicable)
- [ ] Review [`Shared/Filtering/SimpleFilterParser.cs`](./Shared/Filtering/SimpleFilterParser.cs) for PostgreSQL compatibility; add support for both SQL Server and PostgreSQL
- [ ] Refactor services (starting with EmployeeService) to follow Clean Architecture principles
- [ ] Create an attribute for entities/tables that should be excluded from logging/auditing
- [ ] Add metrics to API Gateway (integrate with Prometheus/Grafana)
- [ ] Implement event-driven architecture in NotificationService and add a consumer (e.g., user validation event)
- [ ] Review and add documentation (XML docs / comments) throughout the Shared library
- [ ] Implement all `NotImplementedException` methods in the AuthService
