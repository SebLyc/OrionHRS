using Microsoft.AspNetCore.Mvc;
using OrionHRS.Models;
using System.Linq;
using System;

namespace OrionHRS.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly AppDbContext _context;

        public JobOfferController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Wyświetlenie listy ofert pracy
        public IActionResult Index()
        {
            var jobOffers = _context.JobOffers
                .OrderByDescending(j => j.PostedDate)
                .ToList();

            return View(jobOffers);
        }

        // GET: Wyświetlenie szczegółów oferty
        public IActionResult Details(int id)
        {
            var jobOffer = _context.JobOffers.FirstOrDefault(j => j.ID == id);

            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }
    }
}
