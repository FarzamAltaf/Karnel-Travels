namespace kernel.Models
{
    public class Packages
    {
        public int id { get; set; }
        public string title { get; set; }
        public string duration { get; set; }
        public string location { get; set; }
        public int price { get; set; }
        public string mainImage { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public int maxPeople { get; set; }
        public string description { get; set; }

        public Packages(string title, string duration, string location, int price, string mainImage, string image1, string image2, string image3, string image4, int maxPeople, string description)
        {
            this.title = title;
            this.duration = duration;
            this.location = location;
            this.price = price;
            this.mainImage = mainImage;
            this.image1 = image1;
            this.image2 = image2;
            this.image3 = image3;
            this.image4 = image4;
            this.maxPeople = maxPeople;
            this.description = description;
        }
    }
}
