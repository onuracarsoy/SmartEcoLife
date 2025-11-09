using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartEcoLife.Features.Categories;
using SmartEcoLife.Features.FinancialRecords;
using SmartEcoLife.Features.Goals;
using SmartEcoLife.Features.Users;




namespace SmartEcoLife.Data
{
    public class SmartEcoLifeDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SmartEcoLifeDbContext(DbContextOptions<SmartEcoLifeDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<FinancialRecord> FinancialRecords { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            modelBuilder.Entity<FinancialRecord>()
                .HasOne(fr => fr.Category)
                .WithMany(c => c.FinancialRecords)
                .HasForeignKey(fr => fr.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

           
            modelBuilder.Entity<FinancialRecord>()
                .HasOne(fr => fr.User)
                .WithMany(u => u.FinancialRecords)
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            
                


        }
    }

}

