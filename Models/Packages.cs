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
        public string mapLink { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public string extra3 { get; set; }
        public int extCost1 { get; set; }
        public int extCost2 { get; set; }
        public int extCost3 { get; set; }

        public Packages(string title, string duration, string location, int price, string mainImage, string image1, string image2, string image3, string image4, int maxPeople, string description, string mapLink, string extra1, string extra2, string extra3, int extCost1, int extCost2, int extCost3)
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
            this.mapLink = mapLink;
            this.extra1 = extra1;
            this.extra2 = extra2;
            this.extra3 = extra3;
            this.extCost1 = extCost1;
            this.extCost2 = extCost2;
            this.extCost3 = extCost3;
        }
    }
}
