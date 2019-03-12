using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class LibraryManagementSystemContext : DbContext
    {
        public LibraryManagementSystemContext(DbContextOptions<LibraryManagementSystemContext> options)
            : base(options)
        {
        }

        public DbSet<LMSRepository.Interfaces.Models.AssetType> AssetType { get; set; }
    }
}