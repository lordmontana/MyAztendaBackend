# MyAztendaBackend
Design patterns used 
1.[Repository Pattern](./Shared/Repositories/Abstractions/IRepository.cs)Repository pattern – generic IRepository<T> abstraction with concrete Repository<T> to encapsulate persistence operations (./src/Infrastructure/Repositories)
2. [CQRS](./Shared/Cqrs/Bases/CreateEntityHandler.cs)CQRS + Template Method – base CreateEntityHandler defines the command workflow and defers customization to overridable steps, enabling separate command/query handlers   
3.[Mediator pattern ](./Shared/Cqrs/MiniMediator.cs)Mediator pattern – MiniMediator routes commands and queries to their handlers through dependency injection
4.[Decorator pattern](./Shared/Cqrs/Decorators/LoggingCommandDecorator.cs)Decorator pattern – cross‑cutting concerns (logging, validation) are wrapped around handlers via decorator implementations
5.[Factory](./Shared/Filtering/ParserFactory.cs) Factory ParserFactory.Get<TEntity>(mode) decides at runtime which parser implementation to return, hiding instantiation details from callers
6.[Strategy](./Shared/Filtering/IFilterParser.cs) Strategy –IFilterParser<TEntity> defines the contract. Two concrete strategies implement it: AdvancedFilterParser<TEntity> for dynamic-expression parsing and SimpleFilterParser<TEntity> for reflection-based filtering with whitelisting and type-specific logic
7.[Command pattern](./Shared/Cqrs/Abstractions/ICommandHandler.cs)Command pattern – requests are encapsulated as ICommand objects with dedicated handlers implementing HandleAsync, decoupling invocation from execution logic
8.[Chain of Responsibility pattern](./Shared/Cqrs/DependencyInjection/AddCqrs.cs)Chain of Responsibility – cross‑cutting behavior is layered via decorators registered in sequence, allowing each to process or delegate a command/query through the pipeline
9.[Template pattern no1](./Shared/Cqrs/Bases/PagedSearchHandler.cs) [Template pattern no2] (./Shared/Persistence/BaseDbContext.cs) Template Method – base classes outline the algorithm while letting subclasses fill in steps, seen in both PagedSearchHandler and the auditing-enhanced BaseDbContext.SaveChanges override
10.[Service Locator](./Shared/Cqrs/MiniMediator.cs) Service Locator – MiniMediator dynamically resolves concrete handlers from the dependency container, hiding implementation details from callers
11.[Data Transfer Object (DTO)] (./EmployeeService/DTOs/EmployeeDto.cs )Data Transfer Object (DTO) – simple record types like EmployeeDto carry data between layers without exposing domain entities
12.[Middleware pattern](./Shared/Web/Middleware/GlobalExceptionMiddleware.cs)Middleware pattern – GlobalExceptionMiddleware intercepts HTTP requests, handling errors before passing control along the pipeline
13.[API Gateway pattern](./Shared/Web/Middleware/GlobalExceptionMiddleware.cs)API Gateway pattern – Ocelot configuration files route /customer-gate endpoints to the customer service, centralizing access to multiple microservices
