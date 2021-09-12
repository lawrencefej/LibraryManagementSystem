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
                e.Property(a => a.Street).HasMaxLength(100).IsRequired();
                e.Property(a => a.City).HasMaxLength(50).IsRequired();
                e.Property(a => a.Zipcode).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<State>(e =>
            {
                e.Property(a => a.Name).HasMaxLength(25).IsRequired();
                e.Property(a => a.Abbreviations).HasMaxLength(5).IsRequired();
            });

            modelBuilder.Entity<State>().HasData(State.GetStates());
        }
    }
}
