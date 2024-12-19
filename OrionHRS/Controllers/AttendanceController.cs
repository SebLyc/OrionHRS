using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrionHRS.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrionHRS.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Wyświetlenie formularza zgłoszenia obecności
        [HttpGet]
        public IActionResult ReportAttendance()
        {
            return View();
        }

        // POST: Zgłoszenie obecności
        [HttpPost]
        public IActionResult ReportAttendance(string status)
        {
            var employeeEmail = HttpContext.Session.GetString("EmployeeName");

            var employee = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == employeeEmail);

            if (employee == null)
                return RedirectToAction("Login", "Home");

            var today = DateTime.UtcNow.Date;

            bool alreadyReported = _context.Attendances
                .Any(a => a.EmployeeID == employee.ID && a.Day == today);

            if (alreadyReported)
            {
                TempData["Error"] = "Już zgłosiłeś swoją obecność na dzisiejszy dzień!";
                return RedirectToAction("Profile", "Employee");
            }

            var supervisor = _context.Employees
                .FirstOrDefault(e => e.ID == employee.SupervisorID);

            string confirmedBy = supervisor != null
                ? $"{supervisor.FirstName} {supervisor.LastName}"
                : "Automated by system";

            var attendance = new Attendance
            {
                EmployeeID = employee.ID,
                Day = today,
                Status = status,
                SubmissionStatus = supervisor != null ? "Niezakceptowane" : "Zaakceptowane",
                ConfirmedBy = confirmedBy,
                SubmissionDate = DateTime.UtcNow,
                ApprovalDate = supervisor == null ? DateTime.UtcNow : (DateTime?)null
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return RedirectToAction("Profile", "Employee");
        }

        // GET: Lista zgłoszeń obecności
        [HttpGet]
        public IActionResult AttendanceList()
        {
            var employeeEmail = HttpContext.Session.GetString("EmployeeName");

            var employee = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == employeeEmail);

            if (employee == null)
                return RedirectToAction("Login", "Home");

            // Ładowanie obecności wraz z pracownikiem
            var attendances = _context.Attendances
                .Include(a => a.Employee) // Ładowanie powiązanego pracownika
                .Where(a => a.EmployeeID == employee.ID)
                .ToList();

            return View(attendances);
        }

        // GET: Wyświetlenie oczekujących zgłoszeń obecności do zaakceptowania
        [HttpGet]
        public IActionResult PendingAttendances()
        {
            var employeeEmail = HttpContext.Session.GetString("EmployeeName");

            // Pobranie danych przełożonego
            var supervisor = _context.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == employeeEmail);

            if (supervisor == null)
                return RedirectToAction("Login", "Home");

            // Pobranie obecności oczekujących na akceptację dla podwładnych
            var pendingAttendances = _context.Attendances
                .Include(a => a.Employee) // Ładowanie relacji Employee
                .Where(a => a.SubmissionStatus == "Niezakceptowane"
                            && a.Employee.SupervisorID == supervisor.ID)
                .ToList();

            return View(pendingAttendances);
        }

        // POST: Akceptacja zgłoszenia obecności
        [HttpPost]
        public IActionResult AcceptAttendance(int attendanceId)
        {
            var attendance = _context.Attendances
                .Include(a => a.Employee) // Dodatkowe ładowanie powiązanego pracownika
                .FirstOrDefault(a => a.ID == attendanceId);

            if (attendance != null)
            {
                attendance.SubmissionStatus = "Zaakceptowane";
                attendance.ApprovalDate = DateTime.UtcNow;

                _context.Attendances.Update(attendance);
                _context.SaveChanges();
            }

            return RedirectToAction("PendingAttendances");
        }
    }
}
