using SmartBudgetTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllAsync();
        Task<Expense> GetByIdAsync(int id);
        Task AddAsync(Expense expense);
        // IExpenseRepository.cs
        Task<IEnumerable<Expense>> GetAllWithCategoryAsync();

        Task UpdateAsync(Expense expense);
        Task DeleteAsync(int id);
    }
}
