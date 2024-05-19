using Lab2OOP.DTO;
using Lab2OOP.models;
using Microsoft.EntityFrameworkCore;
using Lab2OOP.DateTimeExtension;

namespace Lab2OOP.Services
{
    public class PurchaseService
    {
        private readonly TheatreContext _context;

        public PurchaseService(TheatreContext context)
        {
            _context = context;
        }
        private async Task<TicketDto> MapToTicketDtoAsync(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                FilmId = ticket.FilmId,
                HallId = ticket.HallId,
                Price = ticket.Price,
                ShowTime = DateTimeExtensions.DateTimeToUnixTimestamp(ticket.ShowTime) //changed
            };
        }

        public async Task<PurchaseDto> MapToPurchaseDtoAsync(Purchase purchase)
        {
            var ticketDtos = new List<TicketDto>();

            var tickets = await _context.Tickets
                .Where(t => t.Purchases.Any(p => p.Id == purchase.Id))
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                var ticketDto = await MapToTicketDtoAsync(ticket);
                if (ticketDto != null)
                {
                    ticketDtos.Add(ticketDto);
                }
            }

            return new PurchaseDto
            {
                Id = purchase.Id,
                UserId = purchase.UserId,
                Tickets = ticketDtos
            };
        }

        public async Task<IEnumerable<PurchaseDto>> GetPurchasesAsync()
        {
            var purchases = await _context.Purchases.ToListAsync();
            var purchaseDtos = new List<PurchaseDto>();

            foreach (var purchase in purchases)
            {
                var purchaseDto = await MapToPurchaseDtoAsync(purchase);
                purchaseDtos.Add(purchaseDto);
            }

            return purchaseDtos;
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

                var ticket = new Ticket
                {
                    FilmId = ticketDto.FilmId,
                    HallId = ticketDto.HallId,
                    Price = ticketDto.Price,
                    ShowTime = DateTimeExtensions.UnixTimeStampToDateTime(ticketDto.ShowTime) //changed
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
                var existingTicket = existingPurchase.Tickets.FirstOrDefault(t => t.FilmId == ticketDto.FilmId && t.HallId == ticketDto.HallId);
                if (existingTicket == null)
                {
                    continue;
                }

                existingTicket.Price = ticketDto.Price;
                existingTicket.ShowTime = DateTimeExtensions.UnixTimeStampToDateTime(ticketDto.ShowTime); //changed
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
            var tickets = await _context.Tickets
                .Where(t => t.Purchases.Any(p => p.Id == Id))
                .ToListAsync();

            if (purchase == null)
            {
                return false;
            }

            foreach(var ticket in tickets)
            {
                _context.Tickets.Remove(ticket);
            }
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
