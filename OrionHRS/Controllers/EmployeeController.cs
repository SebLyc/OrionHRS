using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrionHRS.Models;
using System.Linq;

namespace OrionHRS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        // Konstruktor z wstrzykiwaniem kontekstu bazy danych
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Wyświetlenie profilu pracownika
        [HttpGet]
        public IActionResult Profile()
        {
            var employeeEmail = HttpContext.Session.GetString("EmployeeName");
            if (string.IsNullOrEmpty(employeeEmail))
            {
                return RedirectToAction("Login", "Home");
            }

            var employee = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == employeeEmail);

            if (employee == null)
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.EmployeeName = employee.FirstName + " " + employee.LastName;
            ViewBag.Position = employee.Position;

            return View();
        }

        // POST: Wylogowanie pracownika
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}
