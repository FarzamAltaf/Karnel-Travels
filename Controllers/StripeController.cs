using Microsoft.AspNetCore.Mvc;

namespace kernel.Controllers
{
	public class StripeController : Controller
	{
		private readonly IConfiguration _configuration;

		public StripeController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("api/stripe/publishable-key")]
		public IActionResult GetPublishableKey()
		{
			var publishableKey = _configuration["Stripe:PublishableKey"];
			return Json(new { publishableKey });
		}
	}
}