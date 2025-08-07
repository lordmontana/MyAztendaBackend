using Shared.Repositories.Abstractions;
using Shared.Cqrs.Bases;
using LocationService.Entities.Forms;
namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD
{
    public sealed class DeleteLocationHandler(IRepository<Entities.Forms.Location> repo)
        : DeleteEntityHandler<Entities.Forms.Location , DeleteLocationCommand>(repo )  
    {
        protected override int GetId(DeleteLocationCommand c ) => c.Id;
    }
}
