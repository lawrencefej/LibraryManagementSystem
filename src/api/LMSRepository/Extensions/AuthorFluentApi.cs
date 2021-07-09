using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class AuthorFluentApi
    {
        public static void AddAuthorFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(e =>
            {
                e.Property(a => a.FullName).HasMaxLength(50).IsRequired();
                // e.Property(a => a.Des).HasMaxLength(250);
            });

            modelBuilder.Entity<LibraryAssetAuthor>()
                .HasOne(b => b.Author)
                .WithMany(ba => ba.AuthorAssets)
                .HasForeignKey(bc => bc.AuthorId);
        }
    }
}
