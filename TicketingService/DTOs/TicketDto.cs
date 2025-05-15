namespace TicketingService.DTOs
{
    public class TicketDto
    {
        public string Title { get; set; } = string.Empty; // Title of the ticket
        public string Description { get; set; } = string.Empty; // Detailed description of the ticket
        public string Status { get; set; } = "Open"; // Status of the ticket (e.g., Open, In Progress, Closed)
        public int AssignedTo { get; set; } // ID of the user/employee assigned to the ticket
    }
}
