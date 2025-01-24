namespace kernel.Models
{
    public class hotelRestrictions
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public hotelRestrictions(string title, string description)
        {
            this.title = title;
            this.description = description;
        }
    }
}
