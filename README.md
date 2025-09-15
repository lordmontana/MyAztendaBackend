# MyAztendaBackend
Design patterns used [Repository Pattern](./Shared/Repositories/Abstractions/IRepository.cs)
1.Repository pattern – generic IRepository<T> abstraction with concrete Repository<T> to encapsulate persistence operations (./src/Infrastructure/Repositories)
2. CQRS + Template Method – base CreateEntityHandler defines the command workflow and defers customization to overridable steps, enabling separate command/query handlers   
3.Mediator pattern – MiniMediator routes commands and queries to their handlers through dependency injection
4.Decorator pattern – cross‑cutting concerns (logging, validation) are wrapped around handlers via decorator implementations
5.Factory + Strategy – ParserFactory chooses between advanced and simple implementations of IFilterParser<TEntity> at runtime, allowing interchangeable filter strategies
6.Command pattern – requests are encapsulated as ICommand objects with dedicated handlers implementing HandleAsync, decoupling invocation from execution logic
7.Chain of Responsibility – cross‑cutting behavior is layered via decorators registered in sequence, allowing each to process or delegate a command/query through the pipeline
8.Template Method – base classes outline the algorithm while letting subclasses fill in steps, seen in both PagedSearchHandler and the auditing-enhanced BaseDbContext.SaveChanges override
9.Service Locator – MiniMediator dynamically resolves concrete handlers from the dependency container, hiding implementation details from callers
10.Data Transfer Object (DTO) – simple record types like EmployeeDto carry data between layers without exposing domain entities
11.Middleware pattern – GlobalExceptionMiddleware intercepts HTTP requests, handling errors before passing control along the pipeline
12.API Gateway pattern – Ocelot configuration files route /customer-gate endpoints to the customer service, centralizing access to multiple microservices
