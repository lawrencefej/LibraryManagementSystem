using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMSRepository.Interfaces.Models;

namespace LibraryManagementSystem.Models
{
    public class LibraryManagementSystemContext : DbContext
    {
        public LibraryManagementSystemContext (DbContextOptions<LibraryManagementSystemContext> options)
            : base(options)
        {
        }

        public DbSet<LMSRepository.Interfaces.Models.AssetType> AssetType { get; set; }
    }
}
