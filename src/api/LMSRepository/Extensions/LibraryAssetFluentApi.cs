using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class LibraryAssetFluentApi
    {
        public static void AddLibraryAssetFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LibraryAsset>(e =>
            {
                e.Property(a => a.Title)
                    .HasMaxLength(50)
                    .IsRequired();
                e.Property(a => a.Year)
                    .HasMaxLength(4)
                    .IsRequired();
                e.Property(a => a.Status)
                    .HasColumnType("varchar(10)")
                    .IsRequired();
                e.Property(a => a.Added)
                    .IsRequired();
                e.Property(a => a.NumberOfCopies)
                    .IsRequired();
                e.Property(a => a.CopiesAvailable)
                    .IsRequired();
                e.Property(a => a.Description)
                    .HasMaxLength(250);
                e.Property(a => a.AssetType)
                    .HasColumnType("varchar(10)")
                    .IsRequired();
                e.Property(a => a.ISBN)
                    .HasMaxLength(25);
                e.Property(a => a.DeweyIndex)
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<LibraryAssetAuthor>()
                .HasKey(t => new { t.LibrayAssetId, t.AuthorId });

            modelBuilder.Entity<LibraryAssetAuthor>()
                .HasOne(b => b.LibraryAsset)
                .WithMany(ba => ba.AssetAuthors)
                .HasForeignKey(bc => bc.LibrayAssetId);

            modelBuilder.Entity<LibraryAssetAuthor>()
                .HasOne(la => la.Author)
                .WithMany(l => l.AuthorAssets)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<LibraryAssetCategory>()
                .HasKey(t => new { t.LibrayAssetId, t.CategoryId });

            modelBuilder.Entity<LibraryAssetCategory>()
                .HasOne(b => b.LibraryAsset)
                .WithMany(c => c.AssetCategories)
                .HasForeignKey(d => d.LibrayAssetId);

            modelBuilder.Entity<LibraryAssetCategory>()
                .HasOne(b => b.Category)
                .WithMany(c => c.CategoryAssets)
                .HasForeignKey(d => d.CategoryId);

            modelBuilder.Entity<Category>(e =>
            {
                e.Property(a => a.Name).HasMaxLength(50).IsRequired();
                e.Property(a => a.Description).HasMaxLength(250);
            });
            // modelBuilder.Entity<Category>().HasData(
            //     new Category { Id = 1, Name = "Science", Description = "Paper back books" },
            //     new Category { Id = 2, Name = "Computer", Description = "Video and media" }
            //     );
        }
    }
}
