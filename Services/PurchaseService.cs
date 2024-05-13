using Lab2OOP.DTO;
using Lab2OOP.models;
using Microsoft.EntityFrameworkCore;

namespace Lab2OOP.Services
{
	public class PurchaseService
	{
        private readonly TheatreContext _context;

		public PurchaseService(TheatreContext context)
		{
            _context = context;
        }

        public async Task<PurchaseDto> MapToPurchaseDtoAsync(Purchase purchase)
        {
            var ticketDtos = new List<TicketDto>();
            var ticketIds = new List<Ticket>(); // just a placeholder

            foreach (var ticketId in ticketIds)
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);
                if (ticket != null)
                {
                    ticketDtos.Add(new TicketDto
                    {
                        FilmTitle = ticket.Film.Title,
                        HallName = ticket.Hall.Name,
                        Price = ticket.Price,
                        ShowTime = ticket.ShowTime
                    });
                }
            }

            return new PurchaseDto
            {
                UserId = purchase.UserId,
                Tickets = ticketDtos
            };
        }

        public async Task<IEnumerable<PurchaseDto>> GetPurchasesAsync()
        {
            var purchases = await _context.Purchases.ToListAsync();

            return (IEnumerable<PurchaseDto>)purchases.Select(purchase => MapToPurchaseDtoAsync(purchase));
        }

        public async Task<PurchaseDto> GetPurchaseAsync(Guid id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return null;
            }

            return await MapToPurchaseDtoAsync(purchase).ConfigureAwait(false);
        }

        public async Task<Purchase> CreatePurchaseAsync(PurchaseDto purchaseDto)
        {
            var tickets = new List<Ticket>();

            foreach (var ticketDto in purchaseDto.Tickets)
            {
               
                var film = await _context.Films.FirstOrDefaultAsync(f => f.Title == ticketDto.FilmTitle);
                if (film == null)
                {
                    continue; // Skip adding this ticket
                }

                var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Name == ticketDto.HallName);
                if (hall == null)
                {
                    continue;
                }

                var ticket = new Ticket
                {
                    FilmId = film.Id,
                    HallId = hall.Id,
                    Price = ticketDto.Price,
                    ShowTime = ticketDto.ShowTime
                };

                tickets.Add(ticket);
            }

            var purchase = new Purchase
            {
                UserId = purchaseDto.UserId,
                Tickets = tickets
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<bool> UpdatePurchaseAsync(Guid Id, PurchaseDto purchaseDto)
        {
            var existingPurchase = await _context.Purchases
                        .Include(p => p.Tickets)
                        .FirstOrDefaultAsync(p => p.Id == Id);

            if (existingPurchase == null)
            {
                return false;
            }

            existingPurchase.UserId = purchaseDto.UserId;

            foreach (var ticketDto in purchaseDto.Tickets)
            {
                var film = await _context.Films.FirstOrDefaultAsync(f => f.Title == ticketDto.FilmTitle);
                if (film == null)
                {
                    continue;
                }
                var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Name == ticketDto.HallName);
                if (hall == null)
                {
                    continue;
                }

                var existingTicket = existingPurchase.Tickets.FirstOrDefault(t => t.FilmId == film.Id && t.HallId == hall.Id);
                if (existingTicket == null)
                {
                    continue;
                }

                existingTicket.Price = ticketDto.Price;
                existingTicket.ShowTime = ticketDto.ShowTime;
            }

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

        public async Task<bool> DeletePurchaseAsync(Guid Id)
        {
            var purchase = await _context.Purchases.FirstOrDefaultAsync(p => p.Id == Id);

            if (purchase == null)
            {
                return false;
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

