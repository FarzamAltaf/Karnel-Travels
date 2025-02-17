﻿using kernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Stripe;

namespace kernel.Controllers
{
	public class HomeController : Controller
	{

		private readonly IConfiguration _configuration;



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
			var role = Request.Cookies["role"];
			if (role == "admin")
			{
				return RedirectToAction("Index", "Admin");
			}


            var packages = db.packages
                     .OrderByDescending(p => p.id) 
                     .Take(3) 
                     .ToList();
            return View(packages);
		}


		[HttpPost]
		public IActionResult ProcessPayment(string stripeToken)
		{
			try
			{
				// Get the SecretKey from appsettings.json
				var secretKey = _configuration["Stripe:SecretKey"];
				StripeConfiguration.ApiKey = secretKey;

				var options = new ChargeCreateOptions
				{
					Amount = 1000,
					Currency = "usd",
					Description = "Test Payment",
					Source = stripeToken,
				};

				var service = new ChargeService();
				Charge charge = service.Create(options);

				// Handle the result of the charge
				if (charge.Status == "succeeded")
				{
					TempData["PaymentSuccess"] = "Payment was successful!";
				}
				else
				{
					TempData["PaymentError"] = "Payment failed!";
				}
			}
			catch (Exception ex)
			{
				TempData["PaymentError"] = "An error occurred while processing your payment: " + ex.Message;
			}

			return RedirectToAction("Index");
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



		[HttpPost]
		public IActionResult PackageDetails(int packageId, int checkInId, int maxPeople, int price, IFormFile image)
		{
			var id = Request.Cookies["id"];
			var name = Request.Cookies["username"];
			var email = Request.Cookies["email"];

			var intId = Convert.ToInt32(id);
			var package = db.packages.Find(packageId);
			var packageName = package.title;

			var filename = image.FileName;

			var guidImage = Guid.NewGuid() + "_" + filename;


			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uservisa", guidImage);

			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				image.CopyTo(stream);
			}
			var packageBookingDetails = new packageBooking(
				checkInId,
				intId,
				packageId,
				maxPeople,
				price,
				"pending", 
				0,
				guidImage
			);

			db.packageBooking.Add(packageBookingDetails);
			db.SaveChanges();
			TempData["pBooked"] = "Your package is successfully booked";

			string subject = "Tour Package Booking - Pending Approval";
			string body = $@"
			<html>
				<body>
					<h2>Dear {name},</h2>
					<p>We have received your tour package booking request.</p>
					<p><strong>Package Name:</strong> {packageName}</p>
					<p><strong>Booking Status:</strong> <span style='color: orange; font-weight: bold;'>Pending</span></p>
					<p>Your booking is currently under review. We will notify you as soon as it is approved or denied.</p>
					<p>If you have any questions, feel free to reach out to our support team.</p>
					<br>
					<p>Thank you for choosing <strong>Karnel Travels</strong>!</p>
					<p>Best regards,</p>
					<p><strong>Karnel Travel Support Team</strong></p>
				</body>
			</html>";

			sendEmail(email, subject, body);


			return RedirectToAction("PackageDetails", new { id = packageId });
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

				Response.Cookies.Append("id", user.id.ToString(), option);
				Response.Cookies.Append("username", user.username, option);
				Response.Cookies.Append("email", user.email, option);
				Response.Cookies.Append("role", user.role, option);

				TempData["loggedIn"] = "You are successfully logged in!";

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
			Response.Cookies.Delete("username");
			Response.Cookies.Delete("email");
			Response.Cookies.Delete("role");
			return RedirectToAction("signin");
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

		public IActionResult hotel(int? minPrice, int? maxPrice, string searchName, string selectedCountries, int pageNumber = 1, int pageSize = 3)
		{
			if (pageNumber < 1)
			{
				pageNumber = 1; // Negative ya zero page number na allow karo
			}

			var hotels = db.hotels.Where(h => h.status.ToLower() == "available").AsQueryable();

			if (minPrice.HasValue)
			{
				hotels = hotels.Where(h => h.price >= minPrice.Value);
			}

			if (maxPrice.HasValue)
			{
				hotels = hotels.Where(h => h.price <= maxPrice.Value);
			}

			if (!string.IsNullOrEmpty(searchName))
			{
				hotels = hotels.Where(h => h.name.ToLower().Contains(searchName.ToLower()));
			}

			if (!string.IsNullOrEmpty(selectedCountries))
			{
				var countries = selectedCountries.Split(',').ToList();
				hotels = hotels.Where(h => countries.Contains(h.country));
			}

			var countryCounts = hotels
				.GroupBy(h => h.country)
				.Select(group => new { Country = group.Key, Count = group.Count() })
				.ToList();

			ViewBag.CountryCounts = countryCounts;

			var totalHotels = hotels.Count();

			// Correct calculation for Skip, ensuring no negative offset
			var paginatedHotels = hotels
				.Skip((pageNumber - 1) * pageSize) // Page number 1 will start from 0 offset
				.Take(pageSize)
				.ToList();

			ViewBag.TotalHotels = totalHotels;
			ViewBag.PageNumber = pageNumber;
			ViewBag.PageSize = pageSize;

			return View(paginatedHotels);
		}



		public IActionResult hoteldetails(int id)
		{
			var data = db.hotels.Find(id);
			var images = db.hotelImg.Where(b => b.hotelId == id).ToList();
			ViewBag.hotel = data;
			ViewBag.image = images;

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
		//hotel end


		//hotelbooking start
		[HttpPost]
		public IActionResult hoteldetails(int hotelId, DateTime checkIn, DateTime checkOut, int rooms, int costPerRoom, int days)
		{
			var id = Request.Cookies["id"];
			var intId = Convert.ToInt32(id);
			var total = rooms * costPerRoom * days;
			var booking = new hotelBooking(
				 userId: intId,
				 hotelId: hotelId,
				 checkIn: checkIn,
				 checkOut: checkOut,
				 rooms: rooms,
				 days: days,
				 costPerRoom: costPerRoom,
				 total: total,
				 status: "Pending",
				 fee: 0
			 );

			db.hotelBooking.Add(booking);
			db.SaveChanges();

			return RedirectToAction("hoteldetails", new { id = hotelId });
		}
		//hotelbooking end  



		//Visa start

		public IActionResult visa()
		{
			var visa = db.visa.ToList();
			return View(visa);
		}

		public IActionResult VisaDetails(int id)
		{
			var visaData = db.visa.Find(id);
			var name = Request.Cookies["username"];
			var email = Request.Cookies["email"];
			var uId = Request.Cookies["id"];
			var userId = Convert.ToInt32(uId);
			var visaId = id;
			var phone = "";
			var visaType = "";
			var message = "";
			var status = "";
			var image = "";
			var fee = 0;

			var checkIn = "";
			var checkOut = "";

			var visaBookingModel = new visaBooking(name, email, phone, visaType, message, status, image, fee, visaId, userId, checkIn, checkOut);
			var cookieUser = Request.Cookies["email"];
			if (!string.IsNullOrEmpty(cookieUser))
			{
				var model = new VisaPageModel
				{
					Visa = visaData,
				};

				return View(model);
			}
			else
			{
				return RedirectToAction("Signin");
			}
		}

		[HttpPost]

		public IActionResult VisaDetails(int visaId, int userId, string name, string email, string phone, string visaType, string message, IFormFile image)
		{

			var id = Request.Cookies["id"];

			var intId = Convert.ToInt32(id);

			var cookieEmail = Request.Cookies["email"];

			var status = "pending";
			var fee = 0;

			var filename = image.FileName;

			var guidImage = Guid.NewGuid() + "_" + filename;


			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/passportPic", guidImage);

			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				image.CopyTo(stream);
			}

			var cookieUser = Request.Cookies["email"];
			if (!string.IsNullOrEmpty(cookieUser))
			{

				var checkIn = "";
				var checkOut = "";
				visaBooking data = new visaBooking(name, email, phone, visaType, message, guidImage, status, fee, visaId, intId, checkIn, checkOut);
				db.visaBooking.Add(data);
				db.SaveChanges();
				string subject = "Visa Booking Status Update";
				string body = $@"
				<html>
					<body>
						<h2>Dear {name},</h2>
						<p>Thank you for choosing <strong>Karnel Travels</strong> for your visa booking.</p>
						<p>We would like to inform you that your visa booking is currently <strong>pending</strong>.</p>
						<p>Once your visa is either approved or denied, we will notify you immediately via email.</p>
						<br>
						<p>Thank you for your patience and understanding.</p>
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
				TempData["visaBooked"] = "Your request for booking visa is successfully delivered";
				return RedirectToAction("visaDetails", new { id = visaId });
			}
			else
			{
				return RedirectToAction("Signin");
			}
		}
		//Visa end






		//Contact form start
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
				Contact data = new Contact(name, phone, email, message, intId);
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

				if (!string.IsNullOrEmpty(cookieEmail))
				{
					sendEmail(cookieEmail, subject, body);
				}

				ModelState.Clear();
				TempData["contact"] = "Your message is successfully delivered";
			}
			else
			{
				TempData["contact"] = "Signin to submit your inquiry";
			}

			return View();
		}


		public IActionResult Profile()
		{
			var userIdString = HttpContext.Request.Cookies["id"];

			if (string.IsNullOrEmpty(userIdString))
			{
				return BadRequest("User ID not found in cookies.");
			}

			if (int.TryParse(userIdString, out int userId))
			{
				// Fetch VisaBookings where userId matches
				var visaBookings = db.visaBooking
									  .Where(v => v.userId == userId)
									  .ToList();

				// Fetch HotelBookings where userId matches
				var hotelBookings = db.hotelBooking
									   .Where(h => h.userId == userId)
									   .ToList();

				// Fetch Visa details using visaId from visaBookings
				var visas = db.visa
							  .Where(v => visaBookings.Select(vb => vb.visaId).Contains(v.id))
							  .ToList(); // This will return a list of visa details

				// Fetch Hotel details using hotelId from hotelBookings
				var hotels = db.hotels
							   .Where(h => hotelBookings.Select(hb => hb.hotelId).Contains(h.id))
							   .ToList(); // This will return a list of hotel details

				// Create ViewModel and assign values
				var viewModel = new Profile
				{
					VisaBookings = visaBookings,
					HotelBookings = hotelBookings,
					Visa = visas,  // Now this will be a List<visa>
					Hotel = hotels  // Now this will be a List<Hotel>
				};

				return View(viewModel);
			}
			else
			{
				return BadRequest("Invalid User ID format.");
			}
		}


	}
}