using Lab2OOP.DTO;
using Lab2OOP.models;
using Microsoft.EntityFrameworkCore;

namespace Lab2OOP.Services
{
	public class UserService
	{
        private readonly TheatreContext _context;

        public UserService(TheatreContext context)
		{
            _context = context;
        }
        public UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };
        }
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(user => MapToUserDto(user));
        }

        public async Task<UserDto> GetUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<User> CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Surname = userDto.Surname
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserDto userDto)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Name = userDto.Name;
            existingUser.Surname = userDto.Surname;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteUserAsync(Guid Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

