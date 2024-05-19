using Lab2OOP.models;
using Lab2OOP.DTO;
using Microsoft.EntityFrameworkCore;

namespace Lab2OOP.Services
{
	public class HallService
	{
        private readonly TheatreContext _context;
        public HallService(TheatreContext context)
		{
			_context = context;
		}

		public HallDto MapToHallDto(Hall hall)
		{
			return new HallDto
            {
				Id = hall.Id,
                Name = hall.Name,
                Capacity = hall.Capacity
            };
        }

		public async Task<IEnumerable<HallDto>> GetHallsAsync()
		{
			var halls = await _context.Halls.ToListAsync();

			return halls.Select(hall => MapToHallDto(hall));

		}

		public async Task<HallDto?> GetHallAsync(Guid Id)
		{
			var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == Id);

			if (hall == null)
			{
				return null;
			}
			return MapToHallDto(hall);
		}

		public async Task<Hall> CreateHallAsync(HallDto hallDto)
		{
			var hall = new Hall
			{
				Name = hallDto.Name,
				Capacity = hallDto.Capacity
			};

			_context.Add(hall);
			await _context.SaveChangesAsync();

			return hall;
		}

		public async Task<bool> UpdateHallAsync(Guid Id, HallDto hallDto)
		{
			var existingHall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == Id);

			if (existingHall == null)
			{
				return false;
			}

            existingHall.Name = hallDto.Name;
			existingHall.Capacity = hallDto.Capacity;

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

		public async Task<bool> DeleteHallAsync(Guid Id)
		{
            var hall = await _context.Halls.FirstOrDefaultAsync(f => f.Id == Id);

            if (hall == null)
            {
                return false;
            }

			_context.Halls.Remove(hall);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}

