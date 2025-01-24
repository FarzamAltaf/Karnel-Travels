namespace kernel.Models
{
    public class hotelImg
    {
        public int id { get; set; }
        public string image { get; set; }
        public int hotelId { get; set; }

        public hotelImg(string image, int hotelId)
        {
            this.image = image;
            this.hotelId = hotelId;
        }
    }
}
