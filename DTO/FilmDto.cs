namespace Lab2OOP.DTO
{
    public class FilmDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public long Year { get; set; } //changed for unix timestamp
        public string GenreName {get; set;}
    }
}

