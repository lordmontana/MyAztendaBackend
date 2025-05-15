namespace TicketingService.Entities
{
    public class Ticket
    {
        public int Id { get; set; } // Primary key
        public string Title { get; set; } = string.Empty; // Title of the ticket
        public string Description { get; set; } = string.Empty; // Detailed description of the ticket
        public string Status { get; set; } = "Open"; // Status of the ticket (e.g., Open, In Progress, Closed)
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Date the ticket was created
        public DateTime? ClosedDate { get; set; } // Date the ticket was closed (nullable)
        public int AssignedTo { get; set; } // ID of the user/employee assigned to the ticket
    }
}