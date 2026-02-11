using Microsoft.AspNetCore.Mvc;

namespace Quotes.Controllers
{
    public class NewsletterController : Controller
    {
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe(string name, string email)
        {
            return Content($"THANX {name}! You will be hearing from us :)");
        }
    }
}
