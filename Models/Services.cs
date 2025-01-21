namespace kernel.Models
{
    public class Services
    {
        public int id { get; set; }
        public string serviceTitle { get; set; }
        public string serviceCost { get; set; }

        public Services()
        {
        }

        public Services(string serviceTitle, string serviceCost)
        {
            this.serviceTitle = serviceTitle;
            this.serviceCost = serviceCost;
        }

    }
}
