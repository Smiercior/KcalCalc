using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KcalCalc.Models;

namespace KcalCalc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Person> Persons {get; set;}
        public DbSet<KcalDay> KcalDays {get; set;}
        public DbSet<ProductEntry> ProductEntries {get; set;}
        public DbSet<Product> Products {get; set;}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KcalDay>()
                .HasOne(p => p.Person)
                .WithMany(p => p.KcalDays)
                .HasForeignKey(p => p.PersonID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductEntry>()
                .HasOne(k => k.KcalDay)
                .WithMany(k => k.ProductEntries)
                .HasForeignKey(k => k.KcalDayID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductEntry>()
                .HasOne(pr => pr.Product)
                .WithMany(pr => pr.ProductEntries)
                .HasForeignKey(pr => pr.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}