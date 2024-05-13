using Lab2OOP.models;

namespace Lab2OOP.Models
{
	public class PurchaseTicket
	{
        public Guid PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}

