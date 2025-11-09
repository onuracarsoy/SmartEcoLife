using Microsoft.AspNetCore.Identity;
using SmartEcoLife.Features.FinancialRecords;
using SmartEcoLife.Features.Goals;


namespace SmartEcoLife.Features.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public string PreferredCurrency { get; set; } = "TRY";
        public decimal? MonthlyBudget { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

     
        public ICollection<FinancialRecord>? FinancialRecords { get; set; }
        public ICollection<Goal>? Goals { get; set; }
    }


}
