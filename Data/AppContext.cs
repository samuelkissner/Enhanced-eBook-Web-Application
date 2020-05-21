using Microsoft.EntityFrameworkCore;
using EnhancedEbookWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EnhancedEbookWebApp.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ebook> Ebook { get; set; }
    }
}
