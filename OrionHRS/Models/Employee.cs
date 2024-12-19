using System;

namespace OrionHRS.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Password { get; set; } // Hasło do logowania
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoomNumber { get; set; }
        public bool RemoteWork { get; set; }
        public string EmploymentType { get; set; }
        public DateTime HireDate { get; set; }
        public int AnnualLeaveDays { get; set; }
        public int RemainingLeaveDays { get; set; }

        // Klucz obcy do tabeli Wynagrodzenie
        public int SalaryID { get; set; }

        // Status pracownika
        public string Status { get; set; }

        // Klucze obce do innych pracowników
        public int? SupervisorID { get; set; }
        public int? DeputyID { get; set; }

        // Klucze obce do innych tabel
        public int DivisionID { get; set; } // Pion
        public int PositionLevelID { get; set; } // Poziom stanowiska

        public string Position { get; set; } // Nazwa stanowiska
    }
}
