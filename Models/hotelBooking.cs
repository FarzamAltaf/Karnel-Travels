namespace kernel.Models
{
	public class hotelBooking
	{
		public int id {  get; set; }
		public int userId { get; set; }
		public int hotelId { get; set; }
		public DateTime checkIn { get; set; }
		public DateTime checkOut { get; set; }
		public int rooms { get; set; }
		public int days { get; set; }
		public int costPerRoom { get; set; }
		public int total { get; set; }
		public string status { get; set; }
		public int fee { get; set; }

        public hotelBooking(int userId, int hotelId, DateTime checkIn, DateTime checkOut, int rooms, int days, int costPerRoom, int total, string status, int fee)
        {
            this.userId = userId;
            this.hotelId = hotelId;
            this.checkIn = checkIn;
            this.checkOut = checkOut;
            this.rooms = rooms;
            this.days = days;
            this.costPerRoom = costPerRoom;
            this.total = total;
            this.status = status;
            this.fee = fee;
        }
    }
}
