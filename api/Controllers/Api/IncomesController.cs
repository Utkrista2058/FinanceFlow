using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepo;

        public IncomesController(IIncomeRepository incomeRepo)
        {
            _incomeRepo = incomeRepo;
        }

        // GET: api/incomes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var incomes = await _incomeRepo.GetAllAsync();
            return Ok(incomes);
        }

        // GET: api/incomes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var income = await _incomeRepo.GetByIdAsync(id);
            if (income == null)
                return NotFound();

            return Ok(income);
        }

        // POST: api/incomes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Income income)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _incomeRepo.AddAsync(income);
            return CreatedAtAction(nameof(GetById), new { id = income.Id }, income);
        }

        // PUT: api/incomes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Income income)
        {
            if (id != income.Id)
                return BadRequest("Income ID mismatch.");

            await _incomeRepo.UpdateAsync(income);
            return NoContent();
        }

        // DELETE: api/incomes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _incomeRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
