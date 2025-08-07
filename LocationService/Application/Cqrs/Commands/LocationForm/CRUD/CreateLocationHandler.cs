using EmployeeLocationService;
using LocationService.Persistence;
using Shared.Repositories.Abstractions;
using LocationService.Entities.Forms;
using Location = LocationService.Entities.Forms.Location;

namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD
{
    public sealed class CreateLocationHandler(IRepository<Entities.Forms.Location> repo , ApplicationDbContext db) : LocationSaveHandler<CreateLocationCommand>(repo, db)
    {
        protected override Task<(ActionKind, Location)> PrepareEntityAsync(CreateLocationCommand cmd, CancellationToken ct) =>
            Task.FromResult((ActionKind.Create, new Entities.Forms.Location()));

        /// <summary>
        /// Copies data from the DTO inside <see cref="CreateLocationCommand"/>
        /// into a new <see cref="Location"/> entity instance.
        /// </summary>
        protected override void Map(CreateLocationCommand cmd, Location e, ActionKind action)
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
        protected override int GetId(Location e) => e.Id;
    } 
  
}
