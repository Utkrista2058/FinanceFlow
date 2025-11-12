using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Implementations
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly BudgetDbContext _context;
        public IncomeRepository(BudgetDbContext context) => _context = context;

        public async Task<IEnumerable<Income>> GetAllAsync()
            => await _context.Incomes.ToListAsync();

        public async Task<Income> GetByIdAsync(int id)
            => await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id);
        public async Task AddAsync(Income income)
        {
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Income income)
        {
            _context.Incomes.Update(income);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();
            }
        }
    }
}
