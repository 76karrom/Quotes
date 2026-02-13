using Microsoft.AspNetCore.Mvc;

using Quotes.Models;
using Quotes.Services;

namespace Quotes.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _newsletterService;

        public NewsletterController(INewsletterService newsletterService)
        {
            _newsletterService = newsletterService;
        }

        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Subscribers()
        {
            var subscribers = await _newsletterService.GetActiveSubscribersAsync();

            return View(subscribers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(Subscriber subscriber)
        {
            if (!ModelState.IsValid)
            {
                return View(subscriber);
            }

            var result = await _newsletterService.SignUpForNewsletterAsync(subscriber);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("Email", result.Message);
                return View(subscriber);
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Subscribe));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(string email)
        {
            var result = await _newsletterService.OptOutFromNewsletterAsync(email);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Subscribers));
        }
    }
}
