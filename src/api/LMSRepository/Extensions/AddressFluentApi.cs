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
                e.Property(a => a.Zipcode).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<State>(e =>
            {
                e.Property(a => a.Name).HasMaxLength(25).IsRequired();
                e.Property(a => a.Abbreviations).HasMaxLength(5).IsRequired();
            });

            modelBuilder.Entity<State>().HasData(
                new State { Id = 1, Abbreviations = "AL", Name = "Alabama" },
                new State { Id = 2, Abbreviations = "AK", Name = "Alaska" },
                new State { Id = 3, Abbreviations = "AR", Name = "Arkansas" },
                new State { Id = 4, Abbreviations = "AZ", Name = "Arizona" },
                new State { Id = 5, Abbreviations = "CA", Name = "California" },
                new State { Id = 6, Abbreviations = "CO", Name = "Colorado" },
                new State { Id = 7, Abbreviations = "CT", Name = "Connecticut" },
                new State { Id = 8, Abbreviations = "DC", Name = "District of Columbia" },
                new State { Id = 9, Abbreviations = "DE", Name = "Delaware" },
                new State { Id = 10, Abbreviations = "FL", Name = "Florida" },
                new State { Id = 11, Abbreviations = "GA", Name = "Georgia" },
                new State { Id = 12, Abbreviations = "HI", Name = "Hawaii" },
                new State { Id = 13, Abbreviations = "ID", Name = "Idaho" },
                new State { Id = 14, Abbreviations = "IL", Name = "Illinois" },
                new State { Id = 15, Abbreviations = "IN", Name = "Indiana" },
                new State { Id = 16, Abbreviations = "IA", Name = "Iowa" },
                new State { Id = 17, Abbreviations = "KS", Name = "Kansas" },
                new State { Id = 18, Abbreviations = "KY", Name = "Kentucky" },
                new State { Id = 19, Abbreviations = "LA", Name = "Louisiana" },
                new State { Id = 20, Abbreviations = "ME", Name = "Maine" },
                new State { Id = 21, Abbreviations = "MD", Name = "Maryland" },
                new State { Id = 22, Abbreviations = "MA", Name = "Massachusetts" },
                new State { Id = 23, Abbreviations = "MI", Name = "Michigan" },
                new State { Id = 24, Abbreviations = "MN", Name = "Minnesota" },
                new State { Id = 25, Abbreviations = "MS", Name = "Mississippi" },
                new State { Id = 26, Abbreviations = "MO", Name = "Missouri" },
                new State { Id = 27, Abbreviations = "MT", Name = "Montana" },
                new State { Id = 28, Abbreviations = "NE", Name = "Nebraska" },
                new State { Id = 29, Abbreviations = "NH", Name = "New Hampshire" },
                new State { Id = 30, Abbreviations = "NJ", Name = "New Jersey" },
                new State { Id = 31, Abbreviations = "NM", Name = "New Mexico" },
                new State { Id = 32, Abbreviations = "NY", Name = "New York" },
                new State { Id = 33, Abbreviations = "NC", Name = "North Carolina" },
                new State { Id = 34, Abbreviations = "NV", Name = "Nevada" },
                new State { Id = 35, Abbreviations = "ND", Name = "North Dakota" },
                new State { Id = 36, Abbreviations = "OH", Name = "Ohio" },
                new State { Id = 37, Abbreviations = "OK", Name = "Oklahoma" },
                new State { Id = 38, Abbreviations = "OR", Name = "Oregon" },
                new State { Id = 39, Abbreviations = "PA", Name = "Pennsylvania" },
                new State { Id = 40, Abbreviations = "RI", Name = "Rhode Island" },
                new State { Id = 41, Abbreviations = "SC", Name = "South Carolina" },
                new State { Id = 42, Abbreviations = "SD", Name = "South Dakota" },
                new State { Id = 43, Abbreviations = "TN", Name = "Tennessee" },
                new State { Id = 44, Abbreviations = "TX", Name = "Texas" },
                new State { Id = 45, Abbreviations = "UT", Name = "Utah" },
                new State { Id = 46, Abbreviations = "VT", Name = "Vermont" },
                new State { Id = 47, Abbreviations = "VA", Name = "Virginia" },
                new State { Id = 48, Abbreviations = "WA", Name = "Washington" },
                new State { Id = 49, Abbreviations = "WV", Name = "West Virginia" },
                new State { Id = 50, Abbreviations = "WI", Name = "Wisconsin" },
                new State { Id = 51, Abbreviations = "WY", Name = "Wyoming" }
            );
        }
    }
}
