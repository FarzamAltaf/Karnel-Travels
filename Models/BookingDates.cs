namespace kernel.Models
{
    public class BookingDates
    {
        public int id { get; set; }

        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }

        public int packageId { get; set; }

        public BookingDates(DateTime checkIn, DateTime checkOut, int packageId)
        {
            this.checkIn = checkIn;
            this.checkOut = checkOut;
            this.packageId = packageId;
        }
    }
}
