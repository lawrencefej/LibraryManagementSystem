﻿// <auto-generated />
using System;
using LMSLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LMSLibrary.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190111021558_addcategory2")]
    partial class addcategory2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LMSLibrary.Models.AssetType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("LibraryAssetId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AssetTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Paper back books",
                            LibraryAssetId = 0,
                            Name = "Book"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Video and media",
                            LibraryAssetId = 0,
                            Name = "Media"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Others",
                            LibraryAssetId = 0,
                            Name = "Other"
                        });
                });

            modelBuilder.Entity("LMSLibrary.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LMSLibrary.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("LibraryAssetId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("LMSLibrary.Models.Checkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateReturned");

                    b.Property<bool>("IsReturned");

                    b.Property<int>("LibraryAssetId");

                    b.Property<int>("LibraryCardId");

                    b.Property<DateTime>("Since");

                    b.Property<int?>("StatusId");

                    b.Property<DateTime>("Until");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.HasIndex("StatusId");

                    b.ToTable("Checkouts");
                });

            modelBuilder.Entity("LMSLibrary.Models.CheckoutHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CheckedIn");

                    b.Property<DateTime>("CheckedOut");

                    b.Property<int?>("LibraryAssetId");

                    b.Property<int?>("LibraryCardId");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.ToTable("CheckoutHistory");
                });

            modelBuilder.Entity("LMSLibrary.Models.Hold", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("HoldPlaced");

                    b.Property<int?>("LibraryAssetId");

                    b.Property<int?>("LibraryCardId");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.ToTable("Holds");
                });

            modelBuilder.Entity("LMSLibrary.Models.LibraryAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Added");

                    b.Property<int?>("AssetTypeId");

                    b.Property<int?>("AuthorId");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("CopiesAvailable");

                    b.Property<decimal>("Cost");

                    b.Property<string>("Description");

                    b.Property<string>("DeweyIndex");

                    b.Property<string>("ISBN");

                    b.Property<int>("NumberOfCopies");

                    b.Property<int?>("StatusId");

                    b.Property<string>("Title");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("AssetTypeId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatusId");

                    b.ToTable("LibraryAssets");
                });

            modelBuilder.Entity("LMSLibrary.Models.LibraryCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardNumber");

                    b.Property<DateTime>("Created");

                    b.Property<decimal>("Fees");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("LibraryCards");
                });

            modelBuilder.Entity("LMSLibrary.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Photos");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Photo");
                });

            modelBuilder.Entity("LMSLibrary.Models.ReserveAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateCheckedOut");

                    b.Property<bool>("IsCheckedOut");

                    b.Property<bool>("IsExpired");

                    b.Property<int?>("LibraryAssetId");

                    b.Property<int>("LibraryCardId");

                    b.Property<DateTime>("Reserved");

                    b.Property<int?>("StatusId");

                    b.Property<DateTime>("Until");

                    b.HasKey("Id");

                    b.HasIndex("LibraryAssetId");

                    b.HasIndex("LibraryCardId");

                    b.HasIndex("StatusId");

                    b.ToTable("ReserveAssets");
                });

            modelBuilder.Entity("LMSLibrary.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "7ee05221-6ad2-45c1-ab57-115877897683",
                            Name = "Member",
                            NormalizedName = "MEMBER"
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "9c221167-ec98-49ca-a411-fc64638e2adc",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 3,
                            ConcurrencyStamp = "97ce744a-81ff-4b7d-a747-071216fe5bb0",
                            Name = "Librarian",
                            NormalizedName = "LIBRARIAN"
                        });
                });

            modelBuilder.Entity("LMSLibrary.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "There is a copy available for loan",
                            Name = "Available"
                        },
                        new
                        {
                            Id = 2,
                            Description = "The last available copy has been checked out",
                            Name = "Unavailable"
                        },
                        new
                        {
                            Id = 3,
                            Description = "The last available copy has been checked out",
                            Name = "Checkedout"
                        },
                        new
                        {
                            Id = 4,
                            Description = "The last available copy has been checked out",
                            Name = "Returned"
                        },
                        new
                        {
                            Id = 5,
                            Description = "The last available copy has been checked out",
                            Name = "Expired"
                        },
                        new
                        {
                            Id = 6,
                            Description = "The last available copy has been checked out",
                            Name = "Canceled"
                        });
                });

            modelBuilder.Entity("LMSLibrary.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<string>("Lastname");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("Zipcode");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LMSLibrary.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LMSLibrary.Models.AssetPhoto", b =>
                {
                    b.HasBaseType("LMSLibrary.Models.Photo");

                    b.Property<int>("LibraryAssetId");

                    b.HasIndex("LibraryAssetId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("AssetPhoto");
                });

            modelBuilder.Entity("LMSLibrary.Models.UserProfilePhoto", b =>
                {
                    b.HasBaseType("LMSLibrary.Models.Photo");

                    b.Property<int>("UserId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("UserProfilePhoto");
                });

            modelBuilder.Entity("LMSLibrary.Models.Checkout", b =>
                {
                    b.HasOne("LMSLibrary.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMSLibrary.Models.LibraryCard", "LibraryCard")
                        .WithMany("Checkouts")
                        .HasForeignKey("LibraryCardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMSLibrary.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("LMSLibrary.Models.CheckoutHistory", b =>
                {
                    b.HasOne("LMSLibrary.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId");

                    b.HasOne("LMSLibrary.Models.LibraryCard", "LibraryCard")
                        .WithMany()
                        .HasForeignKey("LibraryCardId");
                });

            modelBuilder.Entity("LMSLibrary.Models.Hold", b =>
                {
                    b.HasOne("LMSLibrary.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId");

                    b.HasOne("LMSLibrary.Models.LibraryCard", "LibraryCard")
                        .WithMany()
                        .HasForeignKey("LibraryCardId");
                });

            modelBuilder.Entity("LMSLibrary.Models.LibraryAsset", b =>
                {
                    b.HasOne("LMSLibrary.Models.AssetType", "AssetType")
                        .WithMany("LibraryAssets")
                        .HasForeignKey("AssetTypeId");

                    b.HasOne("LMSLibrary.Models.Author", "Author")
                        .WithMany("Assets")
                        .HasForeignKey("AuthorId");

                    b.HasOne("LMSLibrary.Models.Category", "Category")
                        .WithMany("Assets")
                        .HasForeignKey("CategoryId");

                    b.HasOne("LMSLibrary.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("LMSLibrary.Models.LibraryCard", b =>
                {
                    b.HasOne("LMSLibrary.Models.User", "User")
                        .WithOne("LibraryCard")
                        .HasForeignKey("LMSLibrary.Models.LibraryCard", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMSLibrary.Models.ReserveAsset", b =>
                {
                    b.HasOne("LMSLibrary.Models.LibraryAsset", "LibraryAsset")
                        .WithMany()
                        .HasForeignKey("LibraryAssetId");

                    b.HasOne("LMSLibrary.Models.LibraryCard", "LibraryCard")
                        .WithMany()
                        .HasForeignKey("LibraryCardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMSLibrary.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("LMSLibrary.Models.UserRole", b =>
                {
                    b.HasOne("LMSLibrary.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMSLibrary.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("LMSLibrary.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("LMSLibrary.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("LMSLibrary.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("LMSLibrary.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMSLibrary.Models.AssetPhoto", b =>
                {
                    b.HasOne("LMSLibrary.Models.LibraryAsset", "LibraryAsset")
                        .WithOne("Photo")
                        .HasForeignKey("LMSLibrary.Models.AssetPhoto", "LibraryAssetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMSLibrary.Models.UserProfilePhoto", b =>
                {
                    b.HasOne("LMSLibrary.Models.User", "User")
                        .WithOne("ProfilePicture")
                        .HasForeignKey("LMSLibrary.Models.UserProfilePhoto", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
