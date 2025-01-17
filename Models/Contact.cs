using System.ComponentModel.DataAnnotations;

namespace kernel.Models
{
    public class Contact
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Invalid name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\+?\d{1,3}[\s-]?(\(\d{3}\)|\d{3})[\s-]?\d{3}[\s-]?\d{4}$", ErrorMessage = "Invalid phone number format.")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Invalid Email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Format")]
        public string email { get; set; }

        [Required(ErrorMessage = "Invalid Message")]
        [MinLength(12, ErrorMessage = "Message must be at least 6 characters long")]
        public string message { get; set; }

        public int userId { get; set; }

        public Contact(string name, string phone, string email, string message, int userId)
        {
            this.name = name;
            this.phone = phone;
            this.email = email;
            this.message = message;
            this.userId = userId;
        }
    }
}
