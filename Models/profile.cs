namespace kernel.Models
{
	public class Profile
	{
		public List<visaBooking> VisaBookings { get; set; }  // List of visa bookings
		public List<hotelBooking> HotelBookings { get; set; }  // List of hotel bookings
		public List<visa> Visa { get; set; }  // List of visa details
		public List<Hotel> Hotel { get; set; }  // List of hotel details
	}
}
