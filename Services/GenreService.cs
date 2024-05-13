using Lab2OOP.DTO;
using Lab2OOP.models;
using Microsoft.EntityFrameworkCore;

namespace Lab2OOP.Services
{
	public class GenreService
	{
        private readonly TheatreContext _context;

        public GenreService(TheatreContext context)
        {
            _context = context;
        }

        public GenreDto MapToGenreDto(Genre genre)
        {
            var genreDto = new GenreDto
            {
                Name = genre.Name
            };

            var films = _context.Films.Where(f => f.GenreId == genre.Id).ToList();

            if (films.Any())
            {
                var filmDtos = films.Select(f => new FilmDto
                {
                    Title = f.Title,
                    Year = f.Year,
                    GenreName = genre.Name
                }).ToList();

                genreDto.Films = filmDtos;
            } 

            return genreDto;
        }

        public async Task<Genre> CreateGenreAsync(GenreDto genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

           return genre;
        }

        public async Task<IEnumerable<GenreDto>> GetGenresAsync()
        {
            var genres = await _context.Genres.ToListAsync();
            return genres.Select(MapToGenreDto);
        }

        public async Task<GenreDto> GetGenreAsync(Guid id)
        {
            var genre = await _context.Genres.FindAsync(id);
            GenreDto genreDto = MapToGenreDto(genre);
            return genreDto;
        }

        public async Task<bool> UpdateGenreAsync(Guid id, GenreDto genreDto)
        {
            var existingGenre = await _context.Genres.FindAsync(id);
            if (existingGenre == null)
            {
                return false;
            }

            existingGenre.Name = genreDto.Name;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteGenreAsync(Guid id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return false;
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

