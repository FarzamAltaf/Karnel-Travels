namespace kernel.Models
{
    public class Hotel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string maplink { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int price { get; set; }
        public int maxRoom { get; set; }

        public Hotel(string name, string country, string city, string address, string maplink, string description, string status, int price, int maxRoom)
        {
            this.name = name;
            this.country = country;
            this.city = city;
            this.address = address;
            this.maplink = maplink;
            this.description = description;
            this.status = status;
            this.price = price;
            this.maxRoom = maxRoom;
        }
    }
}
