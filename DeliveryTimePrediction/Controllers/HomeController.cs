using DeliveryTimePrediction.MLModel;
using DeliveryTimePrediction.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeliveryTimePrediction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("User") == null)//logincheck
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        //post for
        [HttpPost]
        public IActionResult Index(DeliveryData input)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Result = null;
                return View(input);
            }
            try
            {
                float predictedTime = ModelBuilder.Predict(input);
                ViewBag.Result = predictedTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Prediction failed");
                ViewBag.Error = "Something went wrong while predicting.";
            }
            return View(input);
        }
        public IActionResult Privacy()
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