using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrionHRS.Models;
using System.Linq;

namespace OrionHRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Konstruktor z wstrzykiwaniem AppDbContext
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("EmployeeName")))
            {
                return RedirectToAction("Profile", "Employee");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var employee = _context.Employees
                .FirstOrDefault(e => e.Email == email && e.Password == password);

            if (employee != null)
            {
                HttpContext.Session.SetString("EmployeeName", $"{employee.FirstName} {employee.LastName}");
                HttpContext.Session.SetString("Position", employee.Position);

                return RedirectToAction("Profile", "Employee");
            }

            ViewBag.Error = "Nieprawid³owy email lub has³o!";
            return View();
        }
    }
}
