using LazyLoadingInAspNetCoreWebApI.Models;
using Microsoft.EntityFrameworkCore;

namespace LazyLoadingInAspNetCoreWebApI.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}