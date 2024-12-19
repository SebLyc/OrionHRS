using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrionHRS.Models;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrionHRS.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dodawanie nowego dokumentu
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Przesyłanie nowego dokumentu
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.ContentType == "application/pdf")
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var document = new Document
                    {
                        Name = file.FileName,
                        FileData = ms.ToArray(),
                        Status = "Oczekujący",
                        Approver1ID = 1, // Identyfikatory osób zatwierdzających
                        Approver2ID = 2,
                        Approver3ID = 3
                    };

                    _context.Documents.Add(document);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Pending");
        }

        // GET: Lista dokumentów do zatwierdzenia
        public IActionResult Pending()
        {
            var documents = _context.Documents
                .Where(d => d.Status == "Oczekujący")
                .ToList();

            return View(documents);
        }

        // POST: Zatwierdzanie dokumentu przez użytkownika
        [HttpPost]
        public IActionResult Approve(int documentId, int approverId)
        {
            var document = _context.Documents.FirstOrDefault(d => d.ID == documentId);

            if (document != null)
            {
                if (approverId == document.Approver1ID) document.Approver1Approved = true;
                if (approverId == document.Approver2ID) document.Approver2Approved = true;
                if (approverId == document.Approver3ID) document.Approver3Approved = true;

                // Sprawdzenie, czy wszyscy zatwierdzili
                if (document.Approver1Approved && document.Approver2Approved && document.Approver3Approved)
                {
                    document.Status = "Zatwierdzony";
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Pending");
        }
        // GET: Pobieranie dokumentu
        public IActionResult Download(int id)
        {
            // Znalezienie dokumentu o podanym ID
            var document = _context.Documents.FirstOrDefault(d => d.ID == id);

            if (document == null || document.FileData == null)
            {
                return NotFound("Dokument nie istnieje lub nie zawiera pliku.");
            }

            // Zwrócenie pliku PDF do pobrania
            return File(document.FileData, "application/pdf", document.Name);
        }
    }
}
