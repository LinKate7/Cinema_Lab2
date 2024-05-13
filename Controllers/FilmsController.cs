using Microsoft.AspNetCore.Mvc;
using Lab2OOP.Services;
using Lab2OOP.DTO;

namespace Lab2OOP
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmService _filmService;
  
        public FilmsController(FilmService filmService)
        {
            _filmService = filmService;
        }

        // GET: api/Films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetFilms()
        {
            var films = await _filmService.GetFilmsAsync();
            return Ok(films);
        }

        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetFilm(Guid id)
        {
            var film = await _filmService.GetFilmAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            return Ok(film);
        }

        // PUT: api/Films/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(Guid id, FilmDto filmDto)
        {
            if (filmDto == null)
            {
                return BadRequest();
            }

            var success = await _filmService.UpdateFilmAsync(id, filmDto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Films
        [HttpPost]
        public async Task<ActionResult<FilmDto>> PostFilm(FilmDto filmDto)
        {
            try
            {
                var createdFilm = await _filmService.CreateFilmAsync(filmDto);
                var createdFilmDto = _filmService.MapToFilmDto(createdFilm);

                return CreatedAtAction("GetFilm", new { id = createdFilm.Id }, createdFilmDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(Guid id)
        {
            var success = await _filmService.DeleteFilmAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
