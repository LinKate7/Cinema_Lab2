using Lab2OOP.DTO;
using Lab2OOP.models;
using Microsoft.EntityFrameworkCore;

namespace Lab2OOP.Services
{
	public class FilmService
	{
        private readonly TheatreContext _context;
        public FilmService(TheatreContext context)
		{
			_context = context;
		}

        public FilmDto MapToFilmDto(Film film)
        {
			var genre = _context.Genres.FirstOrDefault(g => g.Id == film.GenreId);

			return new FilmDto
			{
				Title = film.Title,
				Year = film.Year,
				GenreName = genre != null ? genre.Name : null

            };
        }

        public async Task<IEnumerable<FilmDto>> GetFilmsAsync()
		{
			var films = await _context.Films
					.Include(f => f.Genre)
					.ToListAsync();

			return films.Select(film => MapToFilmDto(film));

        }

		public async Task<FilmDto?> GetFilmAsync(Guid Id)
		{
			var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == Id);

			if (film == null)
			{
				return null;
			}

			return MapToFilmDto(film);
		}

		public async Task<Film> CreateFilmAsync(FilmDto filmDto)
		{
			string genreName = filmDto.GenreName;
			var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = genreName
                };
                _context.Genres.Add(genre);
            }

            var film = new Film
			{
				Title = filmDto.Title,
				Year = filmDto.Year,
				GenreId = genre.Id
			};
            _context.Films.Add(film);
            await _context.SaveChangesAsync();

			return film;
		}

		public async Task<bool> UpdateFilmAsync(Guid Id,FilmDto filmDto)
		{
			var existingFilm = await _context.Films.FirstOrDefaultAsync(f => f.Id == Id);

			if (existingFilm == null)
			{
				return false;
			}

			var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == filmDto.GenreName);

			if (genre == null)
			{
				return false;
			}

			existingFilm.Title = filmDto.Title;
			existingFilm.Year = filmDto.Year;
			existingFilm.GenreId = genre.Id;

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

		public async Task<bool> DeleteFilmAsync(Guid Id)
		{
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == Id);

			if (film == null)
			{
				return false;
			}

			_context.Films.Remove(film);
			await _context.SaveChangesAsync();

			return true;
        }

    }
}

