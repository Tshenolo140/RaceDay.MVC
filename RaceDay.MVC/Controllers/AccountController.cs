using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _api;
        public AccountController(IApiService api) => _api = api;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var response = await _api.LoginAsync(model);
            if (response == null)
            {
                TempData["Error"] = "Invalid credentials.";
                return View(model);
            }
            HttpContext.Session.SetString("JWTToken", response.Token);
            HttpContext.Session.SetString("UserRole", response.Role);
            HttpContext.Session.SetString("UserId", response.UserId.ToString());

            return response.Role == "Organiser"
                ? RedirectToAction("Dashboard", "Organiser")
                : RedirectToAction("Index", "Participant");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _api.RegisterAsync(model);
            if (success)
            {
                TempData["Success"] = "Registration successful. Please login.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Registration failed. Email might already exist.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}