using Microsoft.AspNetCore.Mvc;
using SmartBudgetTracker.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsightsController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepo;
        private readonly IExpenseRepository _expenseRepo;

        public InsightsController(IIncomeRepository incomeRepo, IExpenseRepository expenseRepo)
        {
            _incomeRepo = incomeRepo;
            _expenseRepo = expenseRepo;
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyInsights()
        {
            var now = DateTime.Now;
            var thisMonth = now.Month;
            var lastMonth = now.AddMonths(-1).Month;
            var year = now.Year;

            var incomes = await _incomeRepo.GetAllAsync();
            var expenses = await _expenseRepo.GetAllAsync();

            var thisMonthIncome = incomes.Where(i => i.Date.Month == thisMonth && i.Date.Year == year).Sum(i => i.Amount);
            var thisMonthExpense = expenses.Where(e => e.Date.Month == thisMonth && e.Date.Year == year).Sum(e => e.Amount);

            var lastMonthExpense = expenses.Where(e => e.Date.Month == lastMonth && e.Date.Year == year).Sum(e => e.Amount);

            decimal savingPercent = 0;
            if (thisMonthIncome > 0)
                savingPercent = ((thisMonthIncome - thisMonthExpense) / thisMonthIncome) * 100;

            string message;
            if (savingPercent > 20)
                message = $"Great job! You saved {savingPercent:F1}% this month 🎉";
            else if (savingPercent > 0)
                message = $"You saved {savingPercent:F1}% this month. Try to reach 20% savings!";
            else
                message = "You spent more than you earned this month. Review your expenses.";

            string compareMessage = "";
            if (lastMonthExpense > 0)
            {
                var changePercent = ((thisMonthExpense - lastMonthExpense) / lastMonthExpense) * 100;
                if (changePercent > 0)
                    compareMessage = $"You spent {changePercent:F1}% more than last month.";
                else
                    compareMessage = $"Nice! You spent {Math.Abs(changePercent):F1}% less than last month.";
            }

            return Ok(new
            {
                ThisMonthIncome = thisMonthIncome,
                ThisMonthExpense = thisMonthExpense,
                SavingPercent = savingPercent,
                Summary = message,
                Comparison = compareMessage
            });
        }
    }
}
