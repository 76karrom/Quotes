using Microsoft.AspNetCore.Mvc;

using Quotes.Models;

namespace Quotes.Controllers
{
    public class NewsletterController : Controller
    {
        private static List<Subscriber> _subscribers = [];

        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Subscribers()
        {
            return View(_subscribers);
        }

        [HttpPost]
        public IActionResult Subscribe(Subscriber subscriber)
        {
            if (!ModelState.IsValid)
            {
                return View(subscriber);
            }

            if (_subscribers.Any(s => s.Email == subscriber.Email))
            {
                ModelState.AddModelError("Email", "Already subscribed :)");
                
                return View(subscriber);
            }

            _subscribers.Add(subscriber);

            TempData["SuccessMessage"] = $"THANX {subscriber.Name}! {subscriber.Email} will get news :)";

            return RedirectToAction(nameof(Subscribe));
        }
    }
}
