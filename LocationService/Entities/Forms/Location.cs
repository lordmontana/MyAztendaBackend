using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationService.Entities.Forms
{
    /// <summary>
    /// Represents any physical place in the system (field, building, room, etc.).
    /// Soil metrics live in <see cref="LocationDetail"/> rows.
    /// </summary>
    [Table("Location")]
    [Index(nameof(IId))]           // inherited, but can still be indexed here
    [Index(nameof(Name))]
    public class Location : BaseEntity
    {
            #region Basic Info -----------------------------------------------------

            /// <summary>
            /// Display name (e.g. "Field A", "Main Warehouse").
            /// </summary>
            [MaxLength(100)]
            public string? Name { get; set; }

            /// <summary>
            /// Free-text notes.
            /// </summary>
            [MaxLength(1000)]
            public string? Description { get; set; }

            /// <summary>
            /// Category label: "field", "building", "room", etc.
            /// </summary>
            [MaxLength(100)]
            public string? Type { get; set; }

        /// <summary>   
        /// Crop type grown here (if applicable).
        /// </summary>
        [MaxLength(200)]
        public string? CropType { get; set; }

        /// <summary>
        /// Crop type Id grown here (if applicable).
        /// </summary>
        /// 
        public int? CropTypeId { get; set; }

        #endregion

            #region Address --------------------------------------------------------

            [MaxLength(200)]
            public string? AddressLine1 { get; set; }

            [MaxLength(200)]
            public string? AddressLine2 { get; set; }

            [MaxLength(100)]
            public string? City { get; set; }

            [MaxLength(100)]
            public string? State { get; set; }

            [MaxLength(100)]
            public string? Country { get; set; }

            [MaxLength(20)]
            public string? Postcode { get; set; }

            #endregion

            #region Geo & Physical -------------------------------------------------

            /// <summary>Latitude (decimal degrees, WGS-84).</summary>
            [Precision(9, 6)]                 // ±90.000000 precision
            public decimal? Latitude { get; set; }

            /// <summary>Longitude (decimal degrees, WGS-84).</summary>
            [Precision(9, 6)]                 // ±180.000000 precision
            public decimal? Longitude { get; set; }

            /// <summary>Altitude above sea level (metres).</summary>
            public double? ElevationMeters { get; set; }

            #endregion

            #region Contact --------------------------------------------------------

            [MaxLength(100)]
            public string? ContactPerson { get; set; }

            [MaxLength(30)]
            public string? ContactPhone { get; set; }

            [MaxLength(255)]
            public string? ContactEmail { get; set; }

            #endregion

            #region Hierarchy & Status ---------------------------------------------

            /// <summary>
            /// Parent location for hierarchical grouping (nullable).
            /// </summary>
            public int? ParentLocationId { get; set; }

            /// <summary>
            /// Active flag.
            /// </summary>
             public bool IsActive { get; set; } = false;

            #endregion

            #region Concurrency ----------------------------------------------------

            /// <summary>
            /// Optimistic-concurrency token (handled automatically by EF).
            /// </summary>
            [Timestamp]
            public byte[]? RowVersion { get; set; }

            #endregion

        
        }
    }

