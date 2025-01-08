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
    }
}
