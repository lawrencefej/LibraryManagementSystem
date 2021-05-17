using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class LibraryCardFluentApi
    {
        public static void AddLibraryCardFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LibraryCard>(e =>
            {
                e.Property(a => a.CardNumber).HasMaxLength(25).IsRequired();
                e.Property(a => a.Fees).IsRequired();
                e.Property(a => a.Created).IsRequired();
            });
        }
    }
}
