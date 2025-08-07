using System.Text.Json.Serialization;

namespace LocationService.DTOs
{
    /// <summary>
    /// Read-only response model for Location endpoints.
    /// </summary>
    public class LocationDto
    {
        // ------------------------------------------------------------------
        //  Constructors
        // ------------------------------------------------------------------

        // Needed for JSON serialisers / model binding
        public LocationDto() { }

        /// <summary>
        /// Complete constructor for manual mapping scenarios.
        /// </summary>
        public LocationDto(
            int id,
            DateTime createdUtc,
            DateTime? updatedUtc,
            string? name,
            string? description,
            string? type,
            string? cropType,
            int? cropTypeId,
            string? addressLine1,
            string? addressLine2,
            string? city,
            string? state,
            string? country,
            string? postcode,
            decimal? latitude,
            decimal? longitude,
            double? elevationMeters,
            string? contactPerson,
            string? contactPhone,
            string? contactEmail,
            int? parentLocationId,
            bool isActive
            /* string? rowVersionBase64 */)          // ← add if you expose RowVersion
        {
            Id = id;
            CreatedUtc = createdUtc;
            UpdatedUtc = updatedUtc;
            Name = name;
            Description = description;
            Type = type;
            CropType = cropType;
            CropTypeId = cropTypeId;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Country = country;
            Postcode = postcode;
            Latitude = latitude;
            Longitude = longitude;
            ElevationMeters = elevationMeters;
            ContactPerson = contactPerson;
            ContactPhone = contactPhone;
            ContactEmail = contactEmail;
            ParentLocationId = parentLocationId;
            IsActive = isActive;
            // RowVersion       = rowVersionBase64;
        }
        // --------------- Keys & audit -----------------

        /// <summary>Database primary key (identity).</summary>
        public int Id { get; init; }

        /// <summary>UTC timestamp when the record was created.</summary>
        public DateTime CreatedUtc { get; init; }

        /// <summary>UTC timestamp of the last update, if any.</summary>
        public DateTime? UpdatedUtc { get; init; }

        // --------------- Basic info -------------------

        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Type { get; init; }

        // --------------- Crop specifics ---------------

        public string? CropType { get; init; }
        public int? CropTypeId { get; init; }

        // --------------- Address ----------------------

        public string? AddressLine1 { get; init; }
        public string? AddressLine2 { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Country { get; init; }
        public string? Postcode { get; init; }

        // --------------- Geo & physical ---------------

        public decimal? Latitude { get; init; }

        public decimal? Longitude { get; init; }

        public double? ElevationMeters { get; init; }

        // --------------- Contact ----------------------

        public string? ContactPerson { get; init; }
        public string? ContactPhone { get; init; }
        public string? ContactEmail { get; init; }

        // --------------- Hierarchy & status -----------

        public int? ParentLocationId { get; init; }
        public bool IsActive { get; init; }

        // --------------- Concurrency token ------------

        /// <summary>
        /// Base-64 value returned so clients can send it back in
        /// <c>If-Match</c> headers for optimistic concurrency.
        /// </summary>
      //  public string? RowVersion { get; init; }
    }
}
