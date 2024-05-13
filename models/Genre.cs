namespace Lab2OOP.models;

public class Genre : BaseModel
{
    public string Name { get; set; }
    public ICollection<Film> Films { get; set; }
}