using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;

namespace SmartBudgetTracker.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly BudgetDbContext _context;

        public UserRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        // Repositories/UserRepository.cs
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
