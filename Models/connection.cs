using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace kernel.Models
{
    public class connection : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-GQ8EFUN\\SQLEXPRESS;Database=Karnel-Travel;Integrated Security=True;TrustServerCertificate=True;");
        }
        public DbSet<Users> users { get; set; }
        public DbSet<Contact> contact { get; set; }
        public DbSet<Packages> packages { get; set; }
    }
}
