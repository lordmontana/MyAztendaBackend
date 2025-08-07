using LocationService.Entities.Forms;
using LocationService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
using Shared.Web.Exceptions;

namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD
{
    public sealed class UpdateLocationHandler(
        IRepository<Entities.Forms.Location> repo, ApplicationDbContext db)
        : LocationSaveHandler<UpdateLocationCommand>(repo, db)
    {
        protected override int GetId(Location entity) => entity.Id; 
    

        protected override async Task<(ActionKind Action, Location Entity)> PrepareEntityAsync(UpdateLocationCommand cmd, CancellationToken ct)
        {
            
                var entity = await db.Location.FindAsync(new object[] { cmd.id }, ct)
                             ?? throw new DomainRuleException("Location not found");
                return (ActionKind.Update, entity);
            
        }
        protected override void Map(UpdateLocationCommand cmd, Location e, ActionKind action)
        {
            // d == DTO that arrived over the wire
            var d = cmd.Location ?? throw new ArgumentNullException(nameof(cmd.Location));
            // ------------------------------------------------------------------
            // Basic info
            // ------------------------------------------------------------------
            e.Name = d.Name?.Trim();
            e.Description = d.Description?.Trim();
            e.Type = d.Type?.Trim();
            // Crop specifics
            e.CropType = d.CropType?.Trim();
            e.CropTypeId = d.CropTypeId;
            // ------------------------------------------------------------------
            // Address
            // ------------------------------------------------------------------
            e.AddressLine1 = d.AddressLine1?.Trim();
            e.AddressLine2 = d.AddressLine2?.Trim();
            e.City = d.City?.Trim();
            e.State = d.State?.Trim();
            e.Country = d.Country?.Trim();
            e.Postcode = d.Postcode?.Trim();
            // ------------------------------------------------------------------
            // Geo & physical
            // ------------------------------------------------------------------
            e.Latitude = d.Latitude;
            e.Longitude = d.Longitude;
            e.ElevationMeters = d.ElevationMeters;
            // ------------------------------------------------------------------
            // Contact
            // ------------------------------------------------------------------
            e.ContactPerson = d.ContactPerson?.Trim();
            e.ContactPhone = d.ContactPhone?.Trim();
            e.ContactEmail = d.ContactEmail?.Trim();
            // ------------------------------------------------------------------
            // Hierarchy & status
            // ------------------------------------------------------------------
            e.ParentLocationId = d.ParentLocationId;
            e.IsActive = d.IsActive;
        }
    }
}
