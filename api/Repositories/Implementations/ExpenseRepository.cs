using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Implementations
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly BudgetDbContext _context;
        public ExpenseRepository(BudgetDbContext context) => _context = context;

        public async Task<IEnumerable<Expense>> GetAllAsync()
            => await _context.Expenses.Include(e => e.Category).ToListAsync();
        // ExpenseRepository.cs
        public async Task<IEnumerable<Expense>> GetAllWithCategoryAsync()
        {
            return await _context.Expenses
                                 .Include(e => e.Category)
                                 .ToListAsync();
        }


        public async Task<Expense> GetByIdAsync(int id)
            => await _context.Expenses.Include(e => e.Category)
                                      .FirstOrDefaultAsync(e => e.Id == id);

        public async Task AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }
    }
}
