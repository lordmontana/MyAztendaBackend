using LocationService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Abstractions;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
using Shared.Web.Exceptions;
namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD
{
    public abstract class LocationSaveHandler<tCommand>(IRepository<Entities.Forms.Location> repo, ApplicationDbContext db)
        : SaveEntityHandler<Entities.Forms.Location, tCommand>(repo) where tCommand : ICommand<int>, ILocationPayload
    {
       protected override async Task BeforeSaveAsync(tCommand cmd, Entities.Forms.Location e, ActionKind action, CancellationToken ct)
        {
            var requestedName = cmd.Location.Name;
            bool taken = await db.Location
                                  .AsNoTracking()
                                  .AnyAsync(loc => loc.Name == requestedName &&
                                                   loc.Id != e.Id, ct);
            if (taken) throw new DomainRuleException($"Location Name '{requestedName}' already exists.");

           // if (action == ActionKind.Create)
           // {
           //     // additional rules for Create action
           //     if (await db.Location.AnyAsync(loc => loc.AddressLine1 == e.AddressLine1, ct))
           //         throw new DomainRuleException($"Address '{e.AddressLine1}' already exists.");
           // }
           // if (action == ActionKind.Update)
           // {
           //     // additional rules for Update action
           //     if (await db.Location.AnyAsync(loc => loc.AddressLine1 == e.AddressLine1 && loc.Id != e.Id, ct))
           //         throw new DomainRuleException($"Address '{e.AddressLine1}' already exists.");
           // }
        }
        protected override Task AfterSaveAsync(Entities.Forms.Location e, ActionKind action, CancellationToken ct)
        {
            // e.g. publish domain event, send mail…
            // if (action == ActionKind.Create) _bus.Publish(new LocationCreated(e.Id));
            return Task.CompletedTask;
        }
    }

}
