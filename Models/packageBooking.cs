namespace kernel.Models
{
    public class packageBooking
    {
        public int id { get; set; }
        public int checkInId { get; set; }
        public int userId { get; set; }
        public int packageId { get; set; }
        public int maxPeople { get; set; }
        public int price { get; set; }
        public string status { get; set; }
        public int fee { get; set; }
        public string image { get; set; }

		public packageBooking(int checkInId, int userId, int packageId, int maxPeople, int price, string status, int fee, string image)
		{
			this.checkInId = checkInId;
			this.userId = userId;
			this.packageId = packageId;
			this.maxPeople = maxPeople;
			this.price = price;
			this.status = status;
			this.fee = fee;
			this.image = image;
		}
	}
}
