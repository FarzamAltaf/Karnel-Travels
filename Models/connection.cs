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
        public DbSet<Faqs> faq { get; set; }
        public DbSet<BookingDates> BookingDates { get; set; }
        public DbSet<Services> services { get; set; }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<PackageService> packageServices { get; set; }
        public DbSet<hotelImg> hotelImg { get; set; }
        public DbSet<hotelRestrictions> hotelRestrictions { get; set; }
        public DbSet<visa> visa { get; set; }
        public DbSet<visaBooking> visaBooking { get; set; }
    }
}
