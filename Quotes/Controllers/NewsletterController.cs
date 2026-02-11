using Microsoft.AspNetCore.Mvc;
using Quotes.Models;

namespace Quotes.Controllers
{
    public class NewsletterController : Controller
    {
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe(Subscriber subscriber)
        {
            ViewBag.Message = $"THANX {subscriber.Name}! {subscriber.Email} will get news :)";

            return View();
        }
    }
}
