using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrionHRS.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace OrionHRS.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly AppDbContext _context;

        public LeaveRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Formularz składania wniosku urlopowego
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Złożenie wniosku urlopowego
        [HttpPost]
        public IActionResult Create(string leaveType, DateTime startDate, DateTime endDate)
        {
            var employeeEmail = HttpContext.Session.GetString("EmployeeName");

            var employee = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == employeeEmail);

            if (employee == null)
                return RedirectToAction("Login", "Home");

            // Konwersja dat na UTC - wymagane przez postgresql
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);


            var supervisor = _context.Employees
                .FirstOrDefault(e => e.ID == employee.SupervisorID);
            string confirmedBy = supervisor != null
                ? $"{supervisor.FirstName} {supervisor.LastName}"
                : "Automated by system";

            var leaveRequest = new LeaveRequest
            {
                EmployeeID = employee.ID,
                LeaveType = leaveType,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Oczekujący",
                ProcessedBy = confirmedBy,
                RequestDate = DateTime.UtcNow // Już w UTC
            };

            _context.LeaveRequests.Add(leaveRequest);
            _context.SaveChanges();

            return RedirectToAction("MyRequests");
        }


        // GET: Lista wniosków złożonych przez pracownika
        public IActionResult MyRequests()
        {
            var employeeEmail = HttpContext.Session.GetString("EmployeeName");

            var employee = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == employeeEmail);

            var requests = _context.LeaveRequests
                .Where(lr => lr.EmployeeID == employee.ID)
                .ToList();

            return View(requests);
        }

        // GET: Wnioski oczekujące do akceptacji przez przełożonego
        public IActionResult PendingRequests()
        {
            var supervisorEmail = HttpContext.Session.GetString("EmployeeName");

            var supervisor = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == supervisorEmail);

            var pendingRequests = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.Status == "Oczekujący" && lr.Employee.SupervisorID == supervisor.ID)
                .ToList();

            return View(pendingRequests);
        }

        // POST: Akceptacja wniosku urlopowego
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var request = _context.LeaveRequests.FirstOrDefault(lr => lr.ID == id);

            if (request != null)
            {
                request.Status = "Zaakceptowany";
                request.ProcessedBy = HttpContext.Session.GetString("EmployeeName");
                request.ProcessedDate = DateTime.UtcNow;

                _context.SaveChanges();
            }

            return RedirectToAction("PendingRequests");
        }

        // POST: Odrzucenie wniosku urlopowego
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var request = _context.LeaveRequests.FirstOrDefault(lr => lr.ID == id);

            if (request != null)
            {
                request.Status = "Odrzucony";
                request.ProcessedBy = HttpContext.Session.GetString("EmployeeName");
                request.ProcessedDate = DateTime.UtcNow;

                _context.SaveChanges();
            }

            return RedirectToAction("PendingRequests");
        }
    }
}
