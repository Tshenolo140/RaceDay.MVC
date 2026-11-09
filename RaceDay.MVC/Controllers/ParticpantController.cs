using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IApiService _api;
        public ParticipantController(IApiService api) => _api = api;

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant") return RedirectToAction("Login", "Account");
            var events = await _api.GetEventsAsync();
            return View(events);
        }

        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant") return RedirectToAction("Login", "Account");
            var results = await _api.GetMyResultsAsync();
            return View(results);
        }
    }
}