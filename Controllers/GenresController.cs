using Microsoft.AspNetCore.Mvc;
using Lab2OOP.DTO;
using Lab2OOP.Services;

namespace Lab2OOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenresController(GenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
        {
            var genres = await _genreService.GetGenresAsync();
            return Ok(genres);
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenre(Guid id)
        {
            var genre = await _genreService.GetGenreAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(Guid id, GenreDto genreDto)
        {
            if (genreDto == null)
            {
                return BadRequest();
            }

            var success = await _genreService.UpdateGenreAsync(id, genreDto);
            if (!success)
            {
                return NotFound(); // genre was not found
            }

            return NoContent(); // genre was successfully updated
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<ActionResult<GenreDto>> PostGenre(GenreDto genreDto)
        {
            try
            {
                var createdGenre = await _genreService.CreateGenreAsync(genreDto);
                var createdGenreDto = _genreService.MapToGenreDto(createdGenre);

                return CreatedAtAction(nameof(GetGenre), new { id = createdGenre.Id }, createdGenreDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            var success = await _genreService.DeleteGenreAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
