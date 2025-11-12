using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Implementations
{
    public class BudgetGoalRepository : IBudgetGoalRepository
    {
        private readonly BudgetDbContext _context;

        public BudgetGoalRepository(BudgetDbContext context)
        {
            _context = context;
        }

        // Get all goals (e.g., monthly targets)
        public async Task<IEnumerable<BudgetGoal>> GetAllAsync()
        {
            return await _context.BudgetGoals.ToListAsync();
        }

        // Get goal by ID
        public async Task<BudgetGoal> GetByIdAsync(int id)
        {
            return await _context.BudgetGoals.FindAsync(id);
        }

        // Add new budget goal
        public async Task AddAsync(BudgetGoal goal)
        {
            _context.BudgetGoals.Add(goal);
            await _context.SaveChangesAsync();
        }

        // Update existing budget goal
        public async Task UpdateAsync(BudgetGoal goal)
        {
            _context.BudgetGoals.Update(goal);
            await _context.SaveChangesAsync();
        }

        // Delete goal
        public async Task DeleteAsync(int id)
        {
            var goal = await _context.BudgetGoals.FindAsync(id);
            if (goal != null)
            {
                _context.BudgetGoals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
