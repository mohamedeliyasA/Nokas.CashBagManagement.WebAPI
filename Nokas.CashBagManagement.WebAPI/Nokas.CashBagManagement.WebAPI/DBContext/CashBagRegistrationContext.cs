using Microsoft.EntityFrameworkCore;
using Nokas.CashBagManagement.WebAPI.Entities;

namespace Nokas.CashBagManagement.WebAPI.DBContext
{
    public class CashBagRegistrationContext : DbContext
    {
        public CashBagRegistrationContext(DbContextOptions<CashBagRegistrationContext> options) : base(options)
        {
                
        }
        public DbSet<CashBagRegistration> CashBagRegistration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashBagRegistration>(entity =>
            {
                entity.OwnsOne(e => e.Registrations, registration =>
                {
                    registration.OwnsOne(r => r.Contracts);
                    registration.OwnsOne(r => r.Notes);
                    registration.OwnsOne(r => r.ExchangeRates);
                    registration.OwnsMany(r => r.Vouchers); 
                });
            });

            // ... any other configuration
        }
    }
}
