using SmartBudgetTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Interfaces
{
    public interface IBudgetGoalRepository
    {
        Task<IEnumerable<BudgetGoal>> GetAllAsync();
        Task<BudgetGoal> GetByIdAsync(int id);
        Task AddAsync(BudgetGoal goal);
        Task UpdateAsync(BudgetGoal goal);
        Task DeleteAsync(int id);
    }
}
