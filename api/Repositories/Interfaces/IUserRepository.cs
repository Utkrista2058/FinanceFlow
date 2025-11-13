using SmartBudgetTracker.Models;

namespace SmartBudgetTracker.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<User> GetByIdAsync(int id);
    }
}
