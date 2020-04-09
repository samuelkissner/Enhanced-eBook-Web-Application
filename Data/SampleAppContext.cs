using Microsoft.EntityFrameworkCore;
using EnhancedEbookWebApp.Models;

namespace EnhancedEbookWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ebook> Ebook { get; set; }
    }
}
