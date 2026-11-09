using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IApiService _api;
        public OrganiserController(IApiService api) => _api = api;

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Account");
            var events = await _api.GetEventsAsync();
            return View(events);
        }

        [HttpGet]
        public IActionResult CreateEvent() => View();

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel model, IFormFile banner)
        {
            if (banner != null && banner.Length > 0)
            {
                model.BannerUrl = await _api.UploadEventBannerAsync(banner);
            }
            await _api.CreateEventAsync(model);
            TempData["Success"] = "Event created successfully.";
            return RedirectToAction("Dashboard");
        }
    }
}