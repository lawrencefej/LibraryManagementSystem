﻿// <auto-generated />
using System;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LMSRepository.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("LMSEntities.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("LMSEntities.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "df038373-a791-4fc0-9328-ee6d4daf5bb1",
                            Name = "Member",
                            NormalizedName = "MEMBER"
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "b7dd1ed5-ebb2-40fa-ac96-669d70211435",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 3,
                            ConcurrencyStamp = "6a9ba63b-a9b7-4cb5-9200-1e8441d142ce",
                            Name = "Librarian",
                            NormalizedName = "LIBRARIAN"
                        });
                });

            modelBuilder.Entity("LMSEntities.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<bool>("IsAccountActivated")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LMSEntities.Models.AppUserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("LMSEntities.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LMSEntities.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("LMSEntities.Models.Checkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CheckoutDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateReturned")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsReturned")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LibraryCardId")
                        .HasColumnType("int");

                    b.Property<byte>("RenewalCount")
                        .HasMaxLength(3)
                        .HasColumnType("tinyint unsigned");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("LibraryCardId");

                    b.ToTable("Checkouts");
                });

            modelBuilder.Entity("LMSEntities.Models.CheckoutHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("CheckedIn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CheckedOut")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LibraryAssetId")
                        .HasColumnType("int");

                    b.Property<int?>("LibraryCardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.ToTable("CheckoutHistory");
                });

            modelBuilder.Entity("LMSEntities.Models.CheckoutItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CheckoutId")
                        .HasColumnType("int");

                    b.Property<int>("LibraryAssetId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CheckoutId");

                    b.HasIndex("LibraryAssetId");

                    b.ToTable("CheckoutItems");
                });

            modelBuilder.Entity("LMSEntities.Models.Hold", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("HoldPlaced")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LibraryAssetId")
                        .HasColumnType("int");

                    b.Property<int?>("LibraryCardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.ToTable("Holds");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Added")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AssetType")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CopiesAvailable")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("DeweyIndex")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("ISBN")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<int>("NumberOfCopies")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Year")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("LibraryAssets");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryAssetAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("LibrayAssetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("LibrayAssetId");

                    b.ToTable("LibraryAssetAuthors");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("CardNumber")
                        .HasMaxLength(25)
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Fees")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.ToTable("LibraryCards");
                });

            modelBuilder.Entity("LMSEntities.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PublicId")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Photos");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Photo");
                });

            modelBuilder.Entity("LMSEntities.Models.ReserveAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCheckedOut")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsCheckedOut")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsExpired")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LibraryAssetId")
                        .HasColumnType("int");

                    b.Property<int>("LibraryCardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Reserved")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Until")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.ToTable("ReserveAssets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LMSEntities.Models.AssetPhoto", b =>
                {
                    b.HasBaseType("LMSEntities.Models.Photo");

                    b.Property<int>("LibraryAssetId")
                        .HasColumnType("int");

                    b.HasIndex("LibraryAssetId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("AssetPhoto");
                });

            modelBuilder.Entity("LMSEntities.Models.UserProfilePhoto", b =>
                {
                    b.HasBaseType("LMSEntities.Models.Photo");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("UserProfilePhoto");
                });

            modelBuilder.Entity("LMSEntities.Models.AppUserRole", b =>
                {
                    b.HasOne("LMSEntities.Models.AppRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMSEntities.Models.AppUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSEntities.Models.Checkout", b =>
                {
                    b.HasOne("LMSEntities.Models.LibraryCard", "LibraryCard")
                        .WithMany("Checkouts")
                        .HasForeignKey("LibraryCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryCard");
                });

            modelBuilder.Entity("LMSEntities.Models.CheckoutHistory", b =>
                {
                    b.HasOne("LMSEntities.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId");

                    b.HasOne("LMSEntities.Models.LibraryCard", "LibraryCard")
                        .WithMany()
                        .HasForeignKey("LibraryCardId");

                    b.Navigation("LibraryAsset");

                    b.Navigation("LibraryCard");
                });

            modelBuilder.Entity("LMSEntities.Models.CheckoutItem", b =>
                {
                    b.HasOne("LMSEntities.Models.Checkout", "Checkout")
                        .WithMany("Items")
                        .HasForeignKey("CheckoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMSEntities.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Checkout");

                    b.Navigation("LibraryAsset");
                });

            modelBuilder.Entity("LMSEntities.Models.Hold", b =>
                {
                    b.HasOne("LMSEntities.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId");

                    b.HasOne("LMSEntities.Models.LibraryCard", "LibraryCard")
                        .WithMany()
                        .HasForeignKey("LibraryCardId");

                    b.Navigation("LibraryAsset");

                    b.Navigation("LibraryCard");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryAsset", b =>
                {
                    b.HasOne("LMSEntities.Models.Category", "Category")
                        .WithMany("Assets")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryAssetAuthor", b =>
                {
                    b.HasOne("LMSEntities.Models.Author", "Author")
                        .WithMany("AuthorAssets")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMSEntities.Models.LibraryAsset", "LibraryAsset")
                        .WithMany("AssetAuthors")
                        .HasForeignKey("LibrayAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("LibraryAsset");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryCard", b =>
                {
                    b.HasOne("LMSEntities.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("LMSEntities.Models.AppUser", "Member")
                        .WithOne("LibraryCard")
                        .HasForeignKey("LMSEntities.Models.LibraryCard", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("LMSEntities.Models.ReserveAsset", b =>
                {
                    b.HasOne("LMSEntities.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMSEntities.Models.LibraryCard", "LibraryCard")
                        .WithMany("ReservedAssets")
                        .HasForeignKey("LibraryCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryAsset");

                    b.Navigation("LibraryCard");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("LMSEntities.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("LMSEntities.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("LMSEntities.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("LMSEntities.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LMSEntities.Models.AssetPhoto", b =>
                {
                    b.HasOne("LMSEntities.Models.LibraryAsset", "LibraryAsset")
                        .WithOne("Photo")
                        .HasForeignKey("LMSEntities.Models.AssetPhoto", "LibraryAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryAsset");
                });

            modelBuilder.Entity("LMSEntities.Models.UserProfilePhoto", b =>
                {
                    b.HasOne("LMSEntities.Models.AppUser", "User")
                        .WithOne("ProfilePicture")
                        .HasForeignKey("LMSEntities.Models.UserProfilePhoto", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSEntities.Models.AppRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("LMSEntities.Models.AppUser", b =>
                {
                    b.Navigation("LibraryCard");

                    b.Navigation("ProfilePicture");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("LMSEntities.Models.Author", b =>
                {
                    b.Navigation("AuthorAssets");
                });

            modelBuilder.Entity("LMSEntities.Models.Category", b =>
                {
                    b.Navigation("Assets");
                });

            modelBuilder.Entity("LMSEntities.Models.Checkout", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryAsset", b =>
                {
                    b.Navigation("AssetAuthors");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("LMSEntities.Models.LibraryCard", b =>
                {
                    b.Navigation("Checkouts");

                    b.Navigation("ReservedAssets");
                });
#pragma warning restore 612, 618
        }
    }
}
