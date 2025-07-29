using Shared.Entities;

namespace Shared.Logging.Enities
{
    public class AuditLog :BaseEntity
    {
        public string TableName { get; set; } = default!;   
        public string EntityName { get; set; } = default!;
        public int EntityId { get; set; }
        public string Operation { get; set; } = default!;   // CREATE | UPDATE | DELETE
        public string PerformedBy { get; set; } = default!;
        public DateTime PerformedAtUtc { get; set; }
        public string ChangesJson { get; set; } = default!;
    }
}
