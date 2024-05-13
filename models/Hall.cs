namespace Lab2OOP.models;

public class Hall : BaseModel
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}