using kernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;
using System.IO.Pipelines;
using System.Security.Cryptography.Xml;
using System.IO;
using System;



namespace pract_eProject.Controllers
{
    public class AdminController : Controller
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




		public IActionResult Index(int page = 1)
        {
            ViewBag.Title = "Kernel Travel Dashboard";

            int pageSize = 4;
            int totalRecords = db.visaBooking.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var bookingData = (from booking in db.visaBooking
                               join visa in db.visa on booking.visaId equals visa.id
                               select new VisaBookingViewModel
                               {
                                   Booking = booking,
                                   Country = visa.country
                               })
                               .OrderByDescending(b => b.Booking.id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            ViewBag.visaCount = db.visa.Count();
            ViewBag.packagesCount = db.packages.Count();
            ViewBag.hotelsCount = db.hotels.Count();
            ViewBag.hvBookingCount = db.visaBooking.Count();

            var role = Request.Cookies["role"];

            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }


            return View(bookingData);
        }





        public IActionResult hotel_upload()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult hotel_upload(string name, string country, string city, string address, string maplink, string description, string status,int price,int maxRoom)
        {
            Hotel hotel = new Hotel(name, country, city, address, maplink, description, status,price,maxRoom);
            db.hotels.Add(hotel);
            db.SaveChanges();
            TempData["message"] = "Hotel added successfully!";
            return RedirectToAction("all_hotel");
        }


        public IActionResult all_hotel(int page = 1)
        {
            try
            {
                int pageSize = 3;
                int totalRecords = db.hotels.Count();
                int totalPages = totalRecords > 0 ? (int)Math.Ceiling((double)totalRecords / pageSize) : 1;

                var role = Request.Cookies["role"];
                if (role != "admin")
                {
                    return RedirectToAction("Index", "Home");
                }

                var hotelData = db.hotels
                                         .OrderByDescending(b => b.id)
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                return View(hotelData);
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }


        public IActionResult hotel_image(int id)
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var hotel = db.hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        public IActionResult hotel_image(int hotelId, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var fileName = Guid.NewGuid() + fileExtension;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hotels", fileName);

                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                hotelImg hotel = new hotelImg(fileName, hotelId);
                db.hotelImg.Add(hotel);
                db.SaveChanges();

                TempData["message"] = "Hotel Image added successfully!";
                return RedirectToAction("all_hotel");
            }

            TempData["error"] = "No image selected or invalid image file!";
            return RedirectToAction("all_hotel");
        }



        public IActionResult hotel_edit(int id)
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var hotel = db.hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        public IActionResult hotel_edit(int id, string name, string address, string maplink, string status,int price,int maxRoom)
        {
            var hotel = db.hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel.name = name;
            hotel.address = address;
            hotel.maplink = maplink;
            hotel.status = status;
            hotel.price = price;
            hotel.maxRoom = maxRoom;


            db.SaveChanges();
            TempData["message"] = "Hotel details updated successfully!";
            return RedirectToAction("all_hotel");
        }

        
        public IActionResult hotel_delete(int id)
        {
            var hotel = db.hotels.Find(id);
            if (hotel != null)
            {
                db.hotels.Remove(hotel);
                db.SaveChanges();
                TempData["message"] = "Hotel deleted successfully!";
            }
            return RedirectToAction("all_hotel");
        }

       
        public IActionResult package_upload()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

       
        [HttpPost]
        public IActionResult package_upload(string title, string duration, string location, int price, IFormFile mainImage, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4, int maxPeople, string description, string mapLink)
        {

            var mainImg = mainImage.FileName;
            var Img1 = mainImage.FileName;
            var Img2 = mainImage.FileName;
            var Img3 = mainImage.FileName;
            var Img4 = mainImage.FileName;

            var guidMain = Guid.NewGuid() + "_" + mainImg;
            var guid1 = Guid.NewGuid() + "_" + Img1;
            var guid2 = Guid.NewGuid() + "_" + Img2;
            var guid3 = Guid.NewGuid() + "_" + Img3;
            var guid4 = Guid.NewGuid() + "_" + Img4;


            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/package", guidMain);
            var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/package", guid1);
            var path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/package", guid2);
            var path3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/package", guid3);
            var path4 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/package", guid4);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                mainImage.CopyTo(stream);
            }
            
            using (FileStream stream = new FileStream(path1, FileMode.Create))
            {
                image1.CopyTo(stream);
            }
            
            using (FileStream stream = new FileStream(path2, FileMode.Create))
            {
                image2.CopyTo(stream);
            }
            
            using (FileStream stream = new FileStream(path3, FileMode.Create))
            {
                image3.CopyTo(stream);
            }
            
            using (FileStream stream = new FileStream(path4, FileMode.Create))
            {
                image4.CopyTo(stream);
            }

            Packages package = new Packages(title, duration, location, price, guidMain, guid1, guid2, guid3, guid4, maxPeople, description, mapLink);
            db.Add(package);
            db.SaveChanges();
            TempData["message"] = "Package added successfully!";
            return RedirectToAction("all_packages");
        }

        public IActionResult all_packages(int page = 1)
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            int pageSize = 3;
            var totalPackages = db.packages.ToList();
            var totalPages = (int)Math.Ceiling((double)totalPackages.Count / pageSize);

            var packages = totalPackages.OrderByDescending(b => b.id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;

            return View(packages);
        }



        public IActionResult package_edit(int id)
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var package = db.packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }
            return View(package);
        }


        [HttpPost]
        public IActionResult package_edit(int id, string title, string duration, string location, int price, int maxPeople, string mapLink)
        {
            var package = db.packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            package.title = title;
            package.duration = duration;
            package.location = location;
            package.price = price;
            package.maxPeople = maxPeople;
            package.mapLink = mapLink;

            db.SaveChanges();
            TempData["message"] = "Package updated successfully!";
            return RedirectToAction("all_packages");
        }

        public IActionResult package_delete(int id)
        {
            var package = db.packages.Find(id);
            if (package != null)
            {
                db.packages.Remove(package);
                db.SaveChanges();
                TempData["message"] = "Package deleted successfully!";
            }
            return RedirectToAction("all_packages");
        }

 

        public IActionResult visa_upload()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

    
        [HttpPost]
        public IActionResult visa_upload(string title, string country, int maxStay, string maxTime, string validity, int price, IFormFile image)
        {

            var fileExtension = Path.GetExtension(image.FileName);
            var fileName = Guid.NewGuid() + fileExtension;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "visa", fileName);

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            visa visa = new visa(title, country, maxStay, maxTime, validity, price, fileName);
            db.Add(visa);
            db.SaveChanges();
            TempData["message"] = "Visa added successfully!";
            return RedirectToAction("all_visa");
        }


        public IActionResult all_visa(int page = 1)
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            int pageSize = 3;
            var totalRecords = db.visa.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var visaData = db.visa.OrderByDescending(b => b.id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(visaData);
        }


        public IActionResult visa_edit(int id)
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var visa = db.visa.Find(id);
            if (visa == null)
            {
                return NotFound();
            }
            return View(visa);
        }

  
        [HttpPost]
        public IActionResult visa_edit(int id, string title, string country, int maxStay, string maxTime, string validity, int price)
        {
            var visa = db.visa.Find(id);
            if (visa == null)
            {
                return NotFound();
            }

            visa.title = title;
            visa.country = country;
            visa.maxStay = maxStay;
            visa.maxTime = maxTime;
            visa.validity = validity;
            visa.price = price;

            db.SaveChanges();
            TempData["message"] = "Visa updated successfully!";
            return RedirectToAction("all_visa");
        }


		public IActionResult visa_approve(int id)
		{
			var visa = db.visaBooking.Find(id);
			if (visa == null)
			{
				return NotFound();
			}

			visa.status = "Approved";
			db.SaveChanges();

			string subject = "Visa Approved: Congratulations!";
			string body = $@"
                <html>
                    <body>
                        <h2>Dear {visa.name},</h2>
                        <p>We are pleased to inform you that your visa application has been approved! Congratulations!</p>
                        <p>We understand this is an exciting step, and we're here to support you in your journey. Your visa is now officially approved, and we hope you have a wonderful experience ahead.</p>
                        <p>If you have any questions or need further assistance, please do not hesitate to contact us.</p>
                        <br>
                        <p>Best regards,</p>
                        <p><strong>Karnel Travels Support Team</strong></p>
                    </body>
                </html>";

			sendEmail(visa.email, subject, body);
			return RedirectToAction("booking_list");
		}

		public IActionResult visa_cancel(int id)
		{
			var visa = db.visaBooking.Find(id);
			if (visa == null)
			{
				return NotFound();
			}

			visa.status = "Cancelled";
			db.SaveChanges();

			string subject = "Visa Application Cancellation Notification";
			string body = $@"
         <html>
             <body>
                 <h2>Dear {visa.name},</h2>
                 <p>We regret to inform you that your visa application has been cancelled. Unfortunately, we are unable to proceed with the approval at this time.</p>
                 <p>If you have any questions or require further assistance regarding your application, please feel free to contact us. We are here to help you in any way we can.</p>
                 <br>
                 <p>We understand this may be disappointing, but we hope you will consider reapplying in the future.</p>
                 <br>
                 <p>Best regards,</p>
                 <p><strong>Karnel Travels Support Team</strong></p>
             </body>
         </html>";

			sendEmail(visa.email, subject, body);
			return RedirectToAction("booking_list");
		}





		public IActionResult visa_delete(int id)
        {
            var visa = db.visa.Find(id);
            if (visa != null)
            {
                db.visa.Remove(visa);
                db.SaveChanges();
                TempData["message"] = "Visa deleted successfully!";
            }
            return RedirectToAction("all_visa");
        }

        public IActionResult dashboard_profile()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var adminProfile = db.users.FirstOrDefault(); 
            return View(adminProfile); 
        }



		public IActionResult booking_list(int page = 1, string status = "All")
		{
			int pageSize = 7;
			int totalRecords;
			IQueryable<VisaBookingViewModel> bookingDataQuery;

			bookingDataQuery = (from booking in db.visaBooking
								join visa in db.visa on booking.visaId equals visa.id
								select new VisaBookingViewModel
								{
									Booking = booking,
									Country = visa.country
								})
								.OrderByDescending(b => b.Booking.id);

			if (status != "All")
			{
				bookingDataQuery = bookingDataQuery.Where(b => b.Booking.status == status);
			}

			totalRecords = bookingDataQuery.Count();
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

			var bookingData = bookingDataQuery
								.Skip((page - 1) * pageSize)
								.Take(pageSize)
								.ToList();

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;
			ViewBag.Status = status;

			var role = Request.Cookies["role"];
			if (role != "admin")
			{
				return RedirectToAction("Index", "Home");
			}

			return View(bookingData);
		}



	}
}
