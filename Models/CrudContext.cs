using Microsoft.EntityFrameworkCore;

namespace CRUDelicious.Models
{
    public class CrudContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public CrudContext(DbContextOptions options) : base(options) { }

        public DbSet<Dishes> Dishes { get; set; }
    }
}