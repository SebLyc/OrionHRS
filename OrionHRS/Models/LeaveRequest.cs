using System;

namespace OrionHRS.Models
{
    public class LeaveRequest
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; } // Klucz obcy do pracownika
        public string LeaveType { get; set; } // Typ urlopu
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // Oczekujący, Zaakceptowany, Odrzucony
        public string ProcessedBy { get; set; } // Przełożony
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }

        // Relacja z pracownikiem
        public Employee Employee { get; set; }
    }
}
