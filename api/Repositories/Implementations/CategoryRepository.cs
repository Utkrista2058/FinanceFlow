using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BudgetDbContext _context;
        public CategoryRepository(BudgetDbContext context) => _context = context;

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.Include(c=>c.Expenses).ToListAsync();

        public async Task<Category> GetByIdAsync(int id)
            => await _context.Categories.Include(c=>c.Expenses).FirstOrDefaultAsync(i => i.Id == id);
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
