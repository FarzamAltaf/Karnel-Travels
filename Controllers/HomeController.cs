
using kernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace kernel.Controllers
{
    public class HomeController : Controller
    {
        connection db = new connection();


        public void sendEmail(string to,string subject, string body)
        {
            MailMessage msg = new MailMessage("farzamaltaf888@gmail.com", to, subject, body);
            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("farzamaltaf888@gmail.com", "epdtxztstqznnsvq"),
                EnableSsl = true
            };
            smtp.Send(msg);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult signup()
        {
            var CookieUser = Request.Cookies["email"];
            if (!string.IsNullOrEmpty(CookieUser))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult signup(string username, string email, string password)
        {
            if (db.users.Any(u => u.email == email))
            {

                string subject = "Alert: Account Creation Attempt";
                string body = $@"
                        <html>
                            <body>
                                <h2>Dear {username},</h2>
                                <p>We noticed an attempt to create an account on our platform using this email address: <strong>{email}</strong>.</p>
                                <p>If this wasn’t you, it’s possible someone else tried to use your email address. For your security, please ensure your email account is secure and change your password if necessary.</p>
                                <br>
                                <p>Thank you for your attention!</p>
                                <p>Best regards,</p>
                                <p><strong>Karnel Travels Support Team</strong></p>
                            </body>
                        </html>";

                TempData["userExist"] = "User with this email already exists.";
                sendEmail(email,subject,body );
                return RedirectToAction("signup");
            }
            else
            {
            var role = "user";
            Users user = new Users(username,email,role,password);
            db.users.Add(user);
            db.SaveChanges();
            TempData["registered"] = "You are successfully registered";

                string subject = "Registration Confirmation";
                string body = $@"
                <html>
                    <body>
                        <h2>Dear {username},</h2>
                        <p>Welcome to <strong>Karnel Travels</strong>!</p>
                        <p>Your account has been successfully created using the email: <strong>{email}</strong>.</p>
                        <p>You can now sign in using your credentials.</p>
                        <p>If you face any issues or need further assistance, please feel free to contact our support team.</p>
                        <br>
                        <p>Thank you for registering!</p>
                        <p>Best regards,</p>
                        <p><strong>Karnel Travel Support Team</strong></p>
                    </body>
                </html>";

                sendEmail(email, subject, body);


                return RedirectToAction("signin");
            }
        }

        public IActionResult signin()
        {
            var CookieUser = Request.Cookies["email"];
            if (!string.IsNullOrEmpty(CookieUser))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult signin(string email, string password)
        {
                var user = db.users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                var option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                // Store user data in cookies
                Response.Cookies.Append("username", user.username, option);
                Response.Cookies.Append("email", user.email, option);
                Response.Cookies.Append("role", user.role, option);

                TempData["loggedIn"] = "You are successfully logged in!";

                // Send login confirmation email
                string subject = "Login Successful";
                string body = $@"
                    <html>
                        <body>
                            <h2>Dear {user.username},</h2>
                            <p>You have successfully logged in to <strong>Karnel Travels</strong>!</p>
                            <p>If you did not attempt to log in, please contact our support team immediately.</p>
                            <p>Thank you for using our services!</p>
                            <br>
                            <p>Best regards,</p>
                            <p><strong>Karnel Travel Support Team</strong></p>
                        </body>
                    </html>";

                sendEmail(user.email, subject, body);

                    return RedirectToAction("Index");
            }
            else
            {
                TempData["loginError"] = "Invalid email or password!";
                return View();
            }

        }

        public IActionResult SignOut()
        {
            // Deleting cookies
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("email");
            Response.Cookies.Delete("role");
            return RedirectToAction("signin"); // Redirect to signin page after logout
        }


        public IActionResult destination()
        {
            return View();
        }

        public IActionResult about()
        {
            return View();
        }

        public IActionResult tour()
        {
            return View();
        }

        public IActionResult hotel()
        {
            return View();
        }

        public IActionResult visa()
        {
            return View();
        }

        public IActionResult activity()
        {
            return View();
        }

        public IActionResult transport()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult contact(string name, string phone, string email, string message)
        {
            Contact data = new Contact(name, phone, email, message);
            db.contact.Add(data);
            db.SaveChanges();

            string subject = "Message Received";
            string body = $@"
                <html>
                    <body>
                        <h2>Dear {name},</h2>
                        <p>Thank you for reaching out to <strong>Karnel Travels</strong>.</p>
                        <p>We have received your message:</p>
                        <blockquote>{message}</blockquote>
                        <p>Our team will get back to you shortly.</p>
                        <br>
                        <p>Best regards,</p>
                        <p><strong>Karnel Travel Support Team</strong></p>
                    </body>
                </html>";
            sendEmail(email, subject, body);

            ModelState.Clear();
            TempData["contact"] = "Your message is successfully delivered";

            return View();
        }

    }
}