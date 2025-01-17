namespace kernel.Models
{
    public class Faqs
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }

        public Faqs(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }
    }
}
