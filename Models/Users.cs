using System.ComponentModel.DataAnnotations;

namespace kernel.Models
{
    public class Users
    {
        public int id { get; set; } 

        [Required(ErrorMessage = "Invalid name")]
        public string username { get; set; } 

        [Required(ErrorMessage = "Invalid Email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Format")]
        public string email { get; set; } 

        public string role { get; set; }

        [Required(ErrorMessage = "Invalid Password")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string password { get; set; } 

        // Constructor
        public Users(string username, string email, string role, string password)
        {
            this.username = username;
            this.email = email;
            this.role = role;
            this.password = password;
        }
    }
}
