using Microsoft.AspNetCore.Mvc;

using System;

namespace OrionHRS.Models
{
    public class JobOffer
    {
        public int ID { get; set; }
        public string PositionName { get; set; } // Nazwa stanowiska
        public string Description { get; set; } // Opis stanowiska
        public string Requirements { get; set; } // Wymagania
        public string SalaryRange { get; set; } // Widełki płacowe (np. "5000 - 7000 PLN")
        public DateTime PostedDate { get; set; } // Data opublikowania oferty
    }
}
