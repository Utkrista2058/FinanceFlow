using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models; // Your DbContext namespace
using System;
using System.IO;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly BudgetDbContext _context;

    public ReportController(BudgetDbContext context)
    {
        _context = context;
    }

    [HttpGet("export-excel")]
    public IActionResult ExportExcel()
    {
        using (var workbook = new XLWorkbook())
        {
            // --- Sheet 1: Monthly Summary ---
            var summarySheet = workbook.AddWorksheet("Monthly Summary");
            summarySheet.Cell(1, 1).Value = "Month";
            summarySheet.Cell(1, 2).Value = "Total Income";
            summarySheet.Cell(1, 3).Value = "Total Expense";

            // Get months from Income & Expense
            var incomeByMonth = _context.Incomes
                .GroupBy(i => i.Date.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            var expenseByMonth = _context.Expenses
                .GroupBy(e => e.Date.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            // Populate summary sheet
            for (int i = 1; i <= 12; i++)
            {
                summarySheet.Cell(i + 1, 1).Value = new DateTime(2025, i, 1).ToString("MMMM"); // Month name
                summarySheet.Cell(i + 1, 2).Value = incomeByMonth.FirstOrDefault(x => x.Month == i)?.Total ?? 0;
                summarySheet.Cell(i + 1, 3).Value = expenseByMonth.FirstOrDefault(x => x.Month == i)?.Total ?? 0;
            }

            // --- Sheet 2: Category Breakdown ---
            var categorySheet = workbook.AddWorksheet("Category Breakdown");
            categorySheet.Cell(1, 1).Value = "Category";
            categorySheet.Cell(1, 2).Value = "Total Expense";

            var categoryTotals = _context.Expenses
                .GroupBy(e => e.Category.Name)
                .Select(g => new { CategoryName = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            int row = 2;
            foreach (var cat in categoryTotals)
            {
                categorySheet.Cell(row, 1).Value = cat.CategoryName;
                categorySheet.Cell(row, 2).Value = cat.Total;
                row++;
            }

            // Auto adjust columns
            summarySheet.Columns().AdjustToContents();
            categorySheet.Columns().AdjustToContents();

            // Return as downloadable file
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BudgetReport.xlsx");
            }
        }
    }
}
