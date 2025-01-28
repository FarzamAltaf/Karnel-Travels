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



namespace pract_eProject.Controllers
{
    public class AdminController : Controller
    {
        connection db = new connection();


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

     
        public IActionResult all_hotel()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var hotelData = db.hotels.ToList();
            return View(hotelData);

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
        public IActionResult hotel_edit(int id, string name, string country, string city, string address, string maplink, string description, string status,int price,int maxRoom)
        {
            var hotel = db.hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel.name = name;
            hotel.country = country;
            hotel.city = city;
            hotel.address = address;
            hotel.maplink = maplink;
            hotel.description = description;
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

            var packages = totalPackages.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
        public IActionResult visa_upload(string title, string country, int maxStay, string maxTime, string validity, int price, string image)
        {
            visa visa = new visa(title, country, maxStay, maxTime, validity, price, image);
            db.Add(visa);
            db.SaveChanges();
            TempData["message"] = "Visa added successfully!";
            return RedirectToAction("all_visa");
        }

        
        public IActionResult all_visa()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var visaData = db.visa.ToList();
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
        public IActionResult visa_edit(int id, string title, string country, int maxStay, string maxTime, string validity, int price, string image)
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
            visa.image = image;

            db.SaveChanges();
            TempData["message"] = "Visa updated successfully!";
            return RedirectToAction("all_visa");
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

        //dashboard profile

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

        //BookingDates list

        public IActionResult booking_list()
        {
            var role = Request.Cookies["role"];
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var bookingData = db.visaBooking.ToList();
            return View(bookingData);
        }


    }
}
