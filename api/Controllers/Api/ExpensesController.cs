using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        // GET: api/expense
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var expenses = await _expenseRepository.GetAllAsync();
        //    return Ok(expenses);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _expenseRepository.GetAllWithCategoryAsync();

            var result = expenses.Select(e => new
            {
                e.Id,
                e.Date,
                e.Amount,
                e.Description,
                e.CategoryId,
                CategoryName = e.Category?.Name
            });

            return Ok(result);
        }

     


        // GET: api/expense/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                return NotFound(new { message = "Expense not found" });

            return Ok(expense);
        }

        // POST: api/expense
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _expenseRepository.AddAsync(expense);
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        // PUT: api/expense/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Expense expense)
        {
            if (id != expense.Id)
                return BadRequest("ID mismatch");

            var existingExpense = await _expenseRepository.GetByIdAsync(id);
            if (existingExpense == null)
                return NotFound();

            existingExpense.Date = expense.Date;
            existingExpense.Amount = expense.Amount;
            existingExpense.Description = expense.Description;
            existingExpense.CategoryId = expense.CategoryId;

            await _expenseRepository.UpdateAsync(existingExpense);
            return NoContent();
        }

        // DELETE: api/expense/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                return NotFound();

            await _expenseRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
