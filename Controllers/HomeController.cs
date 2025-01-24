
using kernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace kernel.Controllers
{
	public class HomeController : Controller
	{
		connection db = new connection();


		public void sendEmail(string to, string subject, string body)
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
			var packages = db.packages.ToList();
			return View(packages);
		}



		public IActionResult packagedetails(int id)
		{
			try
			{
				// Retrieve booking dates
				var bookings = db.BookingDates.Where(b => b.packageId == id).ToList();

				// Retrieve package details with related services
				var packageWithServices = db.packages
					.Where(p => p.id == id)
					.Select(p => new
					{
						Package = p,
						Services = db.packageServices
							.Where(ps => ps.packageId == p.id)
							.Join(db.services,
								  ps => ps.serviceId,
								  s => s.id,
								  (ps, s) => s) // Select only the services
							.ToList()
					}).FirstOrDefault();

				// Retrieve FAQs
				var faqs = db.faq.ToList();

				// Assign data to ViewBag
				ViewBag.book = bookings;
				ViewBag.faq = faqs;
				ViewBag.packages = packageWithServices;

				var CookieUser = Request.Cookies["email"];
				if (!string.IsNullOrEmpty(CookieUser))
				{
					return View();
				}
				else
				{
					return RedirectToAction("signin");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
				return RedirectToAction("ErrorPage");
			}
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
				sendEmail(email, subject, body);
				return RedirectToAction("signup");
			}
			else
			{
				var role = "user";
				Users user = new Users(username, email, role, password);
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
				Response.Cookies.Append("id", user.id.ToString(), option);
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
			var packages = db.packages.ToList();
			return View(packages);
		}

		public IActionResult hotel(int? minPrice, int? maxPrice, string searchName, string selectedCountries)
		{
			var hotels = db.hotels.AsQueryable();

			// Price filter logic
			if (minPrice.HasValue)
			{
				hotels = hotels.Where(h => h.price >= minPrice.Value);
			}

			if (maxPrice.HasValue)
			{
				hotels = hotels.Where(h => h.price <= maxPrice.Value);
			}

			// Name search filter
			if (!string.IsNullOrEmpty(searchName))
			{
				hotels = hotels.Where(h => h.name.ToLower().Contains(searchName.ToLower()));
			}

			// Handle selected countries filter
			if (!string.IsNullOrEmpty(selectedCountries))
			{
				var countries = selectedCountries.Split(',').ToList();
				hotels = hotels.Where(h => countries.Contains(h.country));
			}

			// Group by country and get the count of hotels in each country
			var countryCounts = hotels
				.GroupBy(h => h.country)
				.Select(group => new { Country = group.Key, Count = group.Count() })
				.ToList();

			// Pass country counts to the view
			ViewBag.CountryCounts = countryCounts;

			return View(hotels.ToList());
		}





		public IActionResult hoteldetails(int id)
		{
			var data = db.hotels.Find(id);
			var images = db.hotelImg.Where(b => b.hotelId == id).ToList();
			ViewBag.hotel = data;
			ViewBag.image = images;


			return View();
		}


		public IActionResult visa()
		{
			var visa = db.visa.ToList();
			return View(visa);
		}


		public IActionResult visadetails(int id)
		{
			var data = db.visa.Find(id);
			return View(data);
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
			var id = Request.Cookies["id"];
			var intId = Convert.ToInt32(id);
			var cookieEmail = Request.Cookies["email"];


			if (cookieEmail != null)
			{
				// Contact object create karte hain
				Contact data = new Contact(name, phone, email, message, intId);
				db.contact.Add(data);
				db.SaveChanges();
				// Email send karte hain
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

				if (!string.IsNullOrEmpty(cookieEmail))
				{
					sendEmail(cookieEmail, subject, body);
				}

				ModelState.Clear();
				TempData["contact"] = "Your message is successfully delivered";
			}
			else
			{
				// Agar cookies mein userId nahi ho ya invalid ho
				TempData["contact"] = "Signin to submit your inquiry";
			}

			return View();
		}


	}
}