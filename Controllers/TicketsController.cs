using Microsoft.AspNetCore.Mvc;
using Lab2OOP.Services;
using Lab2OOP.DTO;

namespace Lab2OOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _ticketService.GetTicketsAsync();
            return Ok(tickets);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(Guid id)
        {
            var ticket = await _ticketService.GetTicketAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }    

            return Ok(ticket);
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(Guid id, TicketDto ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest();
            }

            var success = await _ticketService.UpdateTicketAsync(id, ticketDto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<TicketDto>> PostTicket(TicketDto ticketDto)
        {
            try
            {
                var createdTicket = await _ticketService.CreateTicketAsync(ticketDto);
                var createdTicketDto = _ticketService.MapToTicketDto(createdTicket);

                return CreatedAtAction("GetTicket", new { id = createdTicket.Id }, createdTicketDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            var success = await _ticketService.DeleteTicketAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

    }
}
