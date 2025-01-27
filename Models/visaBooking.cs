using System.ComponentModel.DataAnnotations;

namespace kernel.Models
{
    public class visaBooking
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Invalid name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Invalid Email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Format")]
        public string email { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\+?\d{1,3}[\s-]?(\(\d{3}\)|\d{3})[\s-]?\d{3}[\s-]?\d{4}$", ErrorMessage = "Invalid phone number format.")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Select visa type")]
        public string visaType { get; set; }

        [Required(ErrorMessage = "Invalid Message")]
        [MinLength(12, ErrorMessage = "Message must be at least 12 characters long")]
        public string message { get; set; }

        [Required(ErrorMessage = "Enter your image here")]
        public string image { get; set; }

        public string status { get; set; }

        public int fee { get; set; }
        
        public int visaId { get; set; }

        public int userId { get; set; }

        public visaBooking(string name, string email, string phone, string visaType, string message, string status, string image, int fee, int visaId, int userId)
        {
            this.name = name;
            this.email = email;
            this.phone = phone;
            this.visaType = visaType;
            this.message = message;
            this.status = status;
            this.image = image;
            this.fee = fee;
            this.visaId = visaId;
            this.userId = userId;
        }
    }
}
