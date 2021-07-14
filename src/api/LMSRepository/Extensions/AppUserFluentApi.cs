using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class AppUserFluentApi
    {
        public static void AddAppUserFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserRole>(userRole =>
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

            modelBuilder.Entity<AppUser>(e =>
            {
                e.Property(a => a.FirstName).HasMaxLength(25).IsRequired();
                e.Property(a => a.LastName).HasMaxLength(25).IsRequired();
                e.Property(a => a.Created).IsRequired();
            });

            modelBuilder.Entity<AppRole>().HasData(
                    new AppRole { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                    new AppRole { Id = 2, Name = "Admin", NormalizedName = "ADMIN" },
                    new AppRole { Id = 3, Name = "Librarian", NormalizedName = "LIBRARIAN" }
                );
        }
    }
}
