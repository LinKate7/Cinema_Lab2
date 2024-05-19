using System.Text.Json.Serialization;

namespace Lab2OOP.DTO
{
	public class GenreDto
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ICollection<FilmDto> Films { get; set; }
    }
}

