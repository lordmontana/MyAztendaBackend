using CustomerService.Models;

namespace CustomerService.Entities
{
    public class CustomerHistory
    {
        public int Id { get; set; }     // PK
        public int CustomerId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = null!;
        public string FieldName { get; set; } = null!;
        public string OldValue { get; set; } = null!;
        public string NewValue { get; set; } = null!;
        public int? IId { get; set; } // Optional field for tracking changes related to the user

    }
}
