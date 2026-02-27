using Microsoft.AspNetCore.Mvc;
using Quotes.Models;
using Quotes.Storage;
using System.Diagnostics;

namespace Quotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger, IImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            ViewData["HeroImageUrl"] = _imageService.GetImageUrl("hero.jpg");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
