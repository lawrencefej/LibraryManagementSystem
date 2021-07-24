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
                e.Property(a => a.FirstName).HasMaxLength(25).IsRequired();
                e.Property(a => a.LastName).HasMaxLength(25).IsRequired();
                e.Property(a => a.Email).HasMaxLength(100).IsRequired();
                e.HasIndex(a => a.Email).IsUnique();
                e.Property(a => a.PhoneNumber).HasMaxLength(15).IsRequired();
                e.Property(a => a.CardNumber).HasMaxLength(25).IsRequired();
                e.HasIndex(a => a.CardNumber).IsUnique();
                e.Property(a => a.Gender)
                    .HasColumnType("varchar(15)")
                    .IsRequired();
                e.Property(a => a.Status)
                    .HasColumnType("varchar(15)")
                    .IsRequired();
                e.Property(a => a.Fees).IsRequired();
                e.Property(a => a.Created).IsRequired();
            });
        }
    }
}
