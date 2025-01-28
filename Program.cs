using Stripe;

namespace kernel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.UseAuthorization();


            //User Routes start

            app.MapControllerRoute(
            name: "about",
            pattern: "about",
            defaults: new { controller = "Home", action = "about" });

            app.MapControllerRoute(
            name: "contact",
            pattern: "contact",
            defaults: new { controller = "Home", action = "contact" });
            
            app.MapControllerRoute(
            name: "transport",
            pattern: "transport",
            defaults: new { controller = "Home", action = "transport" });

            app.MapControllerRoute(
            name: "activity",
            pattern: "activity",
            defaults: new { controller = "Home", action = "activity" });

            app.MapControllerRoute(
            name: "visa",
            pattern: "visa",
            defaults: new { controller = "Home", action = "visa" });

            app.MapControllerRoute(
            name: "hotel",
            pattern: "hotel",
            defaults: new { controller = "Home", action = "hotel" });

            app.MapControllerRoute(
            name: "tour",
            pattern: "tour",
            defaults: new { controller = "Home", action = "tour" });

            app.MapControllerRoute(
            name: "destination",
            pattern: "destination",
            defaults: new { controller = "Home", action = "destination" });

            app.MapControllerRoute(
            name: "signup",
            pattern: "signup",
            defaults: new { controller = "Home", action = "signup" });

            app.MapControllerRoute(
            name: "signin",
            pattern: "signin",
            defaults: new { controller = "Home", action = "signin" });

            app.MapControllerRoute(
            name: "packagedetails",
            pattern: "packagedetails",
            defaults: new { controller = "Home", action = "packagedetails" });
            
            app.MapControllerRoute(
            name: "visadetails",
            pattern: "visadetails",
            defaults: new { controller = "Home", action = "visadetails" });
            
            app.MapControllerRoute(
            name: "hoteldetails",
            pattern: "hoteldetails",
            defaults: new { controller = "Home", action = "hoteldetails" });

            // User Routes end

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}