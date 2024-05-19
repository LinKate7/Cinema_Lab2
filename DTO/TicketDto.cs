namespace Lab2OOP.DTO
{
	public class TicketDto
	{
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }
        public Guid HallId { get; set; }
        public Decimal Price { get; set; }
        public long ShowTime { get; set; } //unix timestamp
    }
}

