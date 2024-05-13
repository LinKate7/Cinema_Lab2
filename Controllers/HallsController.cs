using Microsoft.AspNetCore.Mvc;
using Lab2OOP.models;
using Lab2OOP.Services;
using Lab2OOP.DTO;

namespace Lab2OOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly HallService _hallService;

        public HallsController(HallService hallService)
        {
            _hallService = hallService;
        }

        // GET: api/Halls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallDto>>> GetHalls()
        {
            var halls = await _hallService.GetHallsAsync();
            return Ok(halls);
        }

        // GET: api/Halls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HallDto>> GetHall(Guid id)
        {
            var hall = await _hallService.GetHallAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            return Ok(hall);
        }

        // PUT: api/Halls/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHall(Guid id, HallDto hallDto)
        {

            if (hallDto == null)
            {
                return BadRequest();
            }

            var success = await _hallService.UpdateHallAsync(id, hallDto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Halls
        [HttpPost]
        public async Task<ActionResult<Hall>> PostHall(HallDto hallDto)
        {
            var createdHall = await _hallService.CreateHallAsync(hallDto);
            var createdHallDto = _hallService.MapToHallDto(createdHall);

            return CreatedAtAction("GetHall", new { id = createdHall.Id }, createdHallDto);
        }

        // DELETE: api/Halls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            var success = await _hallService.DeleteHallAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
