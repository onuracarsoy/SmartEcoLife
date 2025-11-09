using SmartEcoLife.Features.FinancialRecords;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace SmartEcoLife.Features.Categories
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Icon { get; set; }


        public Guid? UserId { get; set; }

   
        public ICollection<FinancialRecord>? FinancialRecords { get; set; }
    }
}