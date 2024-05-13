namespace Lab2OOP.models;

public class Purchase : BaseModel
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}