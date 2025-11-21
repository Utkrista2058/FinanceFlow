using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Models;


namespace SmartBudgetTracker.Data
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options) { }

        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BudgetGoal> BudgetGoals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserFCMToken> UserFCMTokens { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  ADD THIS - Create indexes
            modelBuilder.Entity<UserFCMToken>()
                .HasIndex(t => t.UserId);

            modelBuilder.Entity<UserFCMToken>()
                .HasIndex(t => t.FcmToken);
        }
    }
}
