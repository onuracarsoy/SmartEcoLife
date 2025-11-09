using SmartEcoLife.Features.Categories;
using SmartEcoLife.Features.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartEcoLife.Features.FinancialRecords
{
    public enum RecordType { Income, Expense }

    public class FinancialRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public RecordType Type { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
