using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class CheckoutFluentApi
    {
        public static void AddCheckoutFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckoutItem>(e =>
            {
                e.Property(a => a.Status).HasColumnType("varchar(10)").IsRequired();
            });

            modelBuilder.Entity<Checkout>(e =>
            {
                e.Property(a => a.CheckoutDate).IsRequired();
                e.Property(a => a.DueDate).IsRequired();
                e.Property(a => a.RenewalCount).HasMaxLength(3).IsRequired();
                e.Property(a => a.DateReturned).IsRequired();
                e.Property(a => a.Status).HasColumnType("varchar(10)").IsRequired();
            });
        }
    }
}
