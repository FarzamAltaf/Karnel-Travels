namespace kernel.Models
{
	public class visa
	{
		public int id { get; set; }
		public string title { get; set; }
		public string country { get; set; }
		public int maxStay { get; set; }
		public string maxTime { get; set; }
		public string validity { get; set; }
		public int price { get; set; }
		public string image { get; set; }

		public visa(string title, string country, int maxStay, string maxTime, string validity, int price, string image)
		{
			this.title = title;
			this.country = country;
			this.maxStay = maxStay;
			this.maxTime = maxTime;
			this.validity = validity;
			this.price = price;
			this.image = image;
		}
	}
}
