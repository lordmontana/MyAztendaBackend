## Design Patterns Used

### GoF / Object-Oriented Patterns
1. [Repository Pattern](./Shared/Repositories/Abstractions/IRepository.cs) – generic `IRepository<T>` abstraction with concrete `Repository<T>` to encapsulate persistence operations
2. [Factory](./Shared/Filtering/ParserFactory.cs) – `ParserFactory.Get<TEntity>(mode)` decides at runtime which parser implementation to return
3. [Strategy](./Shared/Filtering/IFilterParser.cs) – `IFilterParser<TEntity>` with `AdvancedFilterParser<TEntity>` vs. `SimpleFilterParser<TEntity>`
4. [Decorator](./Shared/Cqrs/Decorators/LoggingCommandDecorator.cs) – cross-cutting concerns (logging, validation) wrapped around handlers
5. [Command](./Shared/Cqrs/Abstractions/ICommandHandler.cs) – requests encapsulated as `ICommand` objects with dedicated handlers
6. [Chain of Responsibility](./Shared/Cqrs/DependencyInjection/AddCqrs.cs) – decorators registered in sequence to process/delegate requests
7. [Template Method](./Shared/Cqrs/Bases/PagedSearchHandler.cs) – base classes outline algorithms, with steps overridden in subclasses

### CQRS & Mediator
8. [CQRS](./Shared/Cqrs/Bases/CreateEntityHandler.cs) – `CreateEntityHandler` defines command workflow with overridable steps
9. [Mediator](./Shared/Cqrs/MiniMediator.cs) – routes commands/queries to handlers via DI
10. [Service Locator](./Shared/Cqrs/MiniMediator.cs) – dynamic resolution of handlers from the DI container

### DTO & Middleware
11. [DTO](./EmployeeService/DTOs/EmployeeDto.cs) – simple record types carry data between layers
12. [Middleware](./Shared/Web/Middleware/GlobalExceptionMiddleware.cs) – intercepts HTTP requests to handle errors globally

### Architectural Patterns
13. [API Gateway](./ApiGatewayService/ocelot.json) – Ocelot routes `/customer-gate` endpoints to the customer service



##  TODO 

- [ ] Change naming of services to proper generic (e.g., `EmployeeService` → `HumanResources` where applicable)
- [ ] Review [`Shared/Filtering/SimpleFilterParser.cs`](./Shared/Filtering/SimpleFilterParser.cs) for compatibility with PostgreSQL; add support for both SQL Server and PostgreSQL
- [ ] Refactor services (starting with EmployeeService) to follow Clean Architecture principles
- [ ] Create an attribute for entities/tables that should be excluded from logging/auditing
- [ ] Add metrics to API Gateway (integrate with Prometheus/Grafana)
- [ ] Implement event-driven architecture in NotificationService and add a consumer (e.g., user validation event)
- [ ] Review and add documentation (XML docs / comments) throughout the **Shared** library for better maintainability
- [ ] Implement all `NotImplementedException` methods in the **AuthService**
