using employeeManagemement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace employeeManagemement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Remove the DbSet<Employee> property, as it's not needed here

        public DbSet<Question>? Questions { get; set; }
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<Salle>? Salles { get; set; }
        public DbSet<Booking>? Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This is important to include

            modelBuilder.Entity<Booking>()
     .HasOne(b => b.Salle)
     .WithMany(s => s.Bookings)
     .HasForeignKey(b => b.SalleId);

        }
    }
}
