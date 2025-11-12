using System.ComponentModel.DataAnnotations;

namespace SmartBudgetTracker.Models
{
    public class Expense
    {
        public int Id { get; set; } //primary key
        [Required]
        public DateTime Date { get; set; }  //expence date
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; } //amount space
        [StringLength(500)]
        public string Description { get; set; }// optional notes

        //foreign key to category
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


    }
}
