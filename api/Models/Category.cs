using System.Text.Json.Serialization;

namespace SmartBudgetTracker.Models

{
    public class Category
    {
        public int Id { get; set; }   //primary key
        public string Name { get; set; }   //eg food,rent,travel etc

        //navigation property - list of expences in this category
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; }=new List<Expense>(); 

    }
}
