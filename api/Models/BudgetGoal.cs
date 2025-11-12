namespace SmartBudgetTracker.Models
{
    public class BudgetGoal
    {
        public int Id { get; set; }                  // Primary Key
        public string Month { get; set; }            // e.g., "Oct-2025"
        public decimal TargetSaving { get; set; }      // Target amount to save
        public decimal? ActualSaving { get; set; }
    }
}
