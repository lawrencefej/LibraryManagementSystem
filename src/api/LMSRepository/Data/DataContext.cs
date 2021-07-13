using LMSEntities.Models;
using LMSRepository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistory { get; set; }
        public DbSet<LibraryAsset> LibraryAssets { get; set; }
        public DbSet<LibraryAssetAuthor> LibraryAssetAuthors { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ReserveAsset> ReserveAssets { get; set; }
        public DbSet<AssetPhoto> AssetPhotos { get; set; }
        public DbSet<UserProfilePhoto> UserProfilePhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddAddressFluentApi();
            builder.AddAppUserFluentApi();
            builder.AddCheckoutFluentApi();
            builder.AddAuthorFluentApi();
            builder.AddLibraryAssetFluentApi();
            builder.AddLibraryCardFluentApi();
            builder.AddPhotoFluentApi();
        }
    }
}
