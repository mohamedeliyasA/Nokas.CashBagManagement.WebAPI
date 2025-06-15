using Microsoft.EntityFrameworkCore;
using Nokas.CashBagManagement.WebAPI.Entities;

namespace Nokas.CashBagManagement.WebAPI.DBContext
{
    public class BagRegistrationDBContext : DbContext
    {
        public BagRegistrationDBContext(DbContextOptions<BagRegistrationDBContext> options) : base(options)
        {
                
        }
        public DbSet<BagRegistrationEntity> BagRegistration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BagRegistrationEntity>(entity =>
            {
                entity.OwnsOne(e => e.BagRegistration, registration =>
                {
                    registration.OwnsOne(r => r.BankInfo);
                    registration.OwnsOne(r => r.Contracts);
                    registration.OwnsOne(r => r.Notes);
                    registration.OwnsMany(r => r.ExchangeRates);
                    registration.OwnsMany(r => r.ForeignCurrencies);
                    registration.OwnsMany(r => r.Vouchers);
                });
            });
        }
    }
}
