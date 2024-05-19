using Lab2OOP.DTO;
using Lab2OOP.models;
using Microsoft.EntityFrameworkCore;
using Lab2OOP.DateTimeExtension;

namespace Lab2OOP.Services
{
    public class TicketService
    {
        private readonly TheatreContext _context;
        public TicketService(TheatreContext context)
        {
            _context = context;
        }

        public TicketDto MapToTicketDto(Ticket ticket)
        {
            var film = _context.Films.FirstOrDefault(f => f.Id == ticket.FilmId);
            var hall = _context.Halls.FirstOrDefault(h => h.Id == ticket.HallId);

            if (film == null || hall == null)
            {
                return null;
            }

            var ticketDto = new TicketDto
            {
                Id = ticket.Id,
                FilmId= film.Id,
                HallId = hall.Id,
                Price = ticket.Price,
                ShowTime = DateTimeExtensions.DateTimeToUnixTimestamp(ticket.ShowTime)//changed
            };

            return ticketDto;
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsAsync()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return tickets.Select(ticket => MapToTicketDto(ticket));
        }

        public async Task<TicketDto?> GetTicketAsync(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            return ticket != null ? MapToTicketDto(ticket) : null;
        }

        public async Task<Ticket?> CreateTicketAsync(TicketDto ticketDto)
        {
            var ticket = new Ticket
            {
                FilmId = ticketDto.FilmId,
                HallId = ticketDto.HallId,
                Price = ticketDto.Price,
                ShowTime = DateTimeExtensions.UnixTimeStampToDateTime(ticketDto.ShowTime)  //changed
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<bool> UpdateTicketAsync(Guid Id, TicketDto ticketDto)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == Id);

            if (ticket == null)
            {
                return false;
            }
   
            ticket.FilmId = ticketDto.FilmId;
            ticket.HallId = ticketDto.HallId;
            ticket.Price = ticketDto.Price;
            ticket.ShowTime = DateTimeExtensions.UnixTimeStampToDateTime(ticketDto.ShowTime); //changed

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

        public async Task<bool> DeleteTicketAsync(Guid Id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == Id);

            if (ticket == null)
            {
                return false;
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}