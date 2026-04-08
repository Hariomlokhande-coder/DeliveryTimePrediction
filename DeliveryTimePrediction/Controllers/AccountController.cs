using DeliveryTimePrediction.Data;
using DeliveryTimePrediction.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTimePrediction.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        // GET Login
        public IActionResult Login()
        {
            return View();
        }

        // GET Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "User already exists";
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System",
                IsDeleted = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
        // POST Login
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(x =>
                x.Email == model.Email &&
                x.IsDeleted == false);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ViewBag.Error = "Invalid email or password";
                return View(model);
            }
            HttpContext.Session.SetString("User", user.Email);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}