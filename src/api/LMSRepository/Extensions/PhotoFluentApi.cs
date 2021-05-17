using LMSEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Extensions
{
    public static class PhotoFluentApi
    {
        public static void AddPhotoFluentApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>(e =>
            {
                e.Property(a => a.Url).HasMaxLength(50).IsRequired();
                e.Property(a => a.DateAdded).IsRequired();
            });

        }
    }
}
