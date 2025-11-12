using SmartBudgetTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<IEnumerable<Income>> GetAllAsync();
        Task<Income> GetByIdAsync(int id);
        Task AddAsync(Income income);
        Task UpdateAsync(Income income);
        Task DeleteAsync(int id);
    }
}
