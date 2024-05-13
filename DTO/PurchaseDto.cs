namespace Lab2OOP.DTO
{
	public class PurchaseDto
	{
        public Guid UserId { get; set; }
        public ICollection<TicketDto> Tickets { get; set; }
    }
}

