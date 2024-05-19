using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2OOP.models;

public class Ticket : BaseModel
{
    [ForeignKey("Film")]
    public Guid FilmId { get; set; }
    [ForeignKey("Hall")]
    public Guid HallId { get; set; }
    public Decimal Price { get; set; }
    public DateTime ShowTime { get; set; } 
    public Film Film { get; set; }
    public Hall Hall { get; set; }
    public ICollection<Purchase> Purchases { get; set; }

}