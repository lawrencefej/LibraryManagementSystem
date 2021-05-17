using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class AddressFluentApi
    {
        public static void AddAddressFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(e =>
            {
                e.Property(a => a.Street).HasMaxLength(50).IsRequired();
                e.Property(a => a.City).HasMaxLength(25).IsRequired();
                e.Property(a => a.State).HasColumnType("nvarchar(2)").IsRequired();
                e.Property(a => a.Zipcode).HasMaxLength(10).IsRequired();
            });
        }
    }
}
