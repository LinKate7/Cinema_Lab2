using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lab2OOP.models;

public class Film : BaseModel
{
    public string Title { get; set; }
    public DateTime Year { get; set; }
    [ForeignKey("Genre")]
    public Guid GenreId { get; set; }
    [JsonIgnore]
    public Genre Genre { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}