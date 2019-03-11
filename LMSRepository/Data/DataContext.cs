using LMSRepository.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistory { get; set; }
        public DbSet<LibraryAsset> LibraryAssets { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ReserveAsset> ReserveAssets { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<AssetPhoto> AssetPhotos { get; set; }
        public DbSet<UserProfilePhoto> UserProfilePhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            //builder.Entity<AssetType>()
            //    .HasMany(c => c.LibraryAssets)
            //    .WithOne(l => l.AssetType);

            builder.Entity<Role>().HasData(
                    new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                    new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" },
                    new Role { Id = 3, Name = "Librarian", NormalizedName = "LIBRARIAN" }
                );

            builder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Available", Description = "There is a copy available for loan" },
                new Status { Id = 2, Name = "Unavailable", Description = "The last available copy has been checked out" },
                new Status { Id = 3, Name = "Checkedout", Description = "" },
                new Status { Id = 4, Name = "Reserved", Description = "" },
                new Status { Id = 5, Name = "Canceled", Description = "" },
                new Status { Id = 6, Name = "Returned", Description = "" },
                new Status { Id = 7, Name = "Expired", Description = "" },
                new Status { Id = 8, Name = "Canceled", Description = "" }
                );

            builder.Entity<AssetType>().HasData(
                new AssetType { Id = 1, Name = "Book", Description = "Paper back books" },
                new AssetType { Id = 2, Name = "Media", Description = "Video and media" },
                new AssetType { Id = 3, Name = "Other", Description = "Others" }
                );

            builder.Entity<Category>().HasData(
                new AssetType { Id = 1, Name = "Science", Description = "Paper back books" },
                new AssetType { Id = 2, Name = "Computer", Description = "Video and media" }
                );
        }
    }
}