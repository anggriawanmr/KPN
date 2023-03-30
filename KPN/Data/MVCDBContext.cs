using KPN.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KPN.Data
{
    public class MVCDBContext : DbContext
    {
        public MVCDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Cat> Cats { get; set; }
    }
}
