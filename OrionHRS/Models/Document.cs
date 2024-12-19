using Microsoft.AspNetCore.Mvc;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrionHRS.Models
{
    public class Document
    {
        public int ID { get; set; } // Identyfikator dokumentu
        public string Name { get; set; } // Nazwa dokumentu

        [Column(TypeName = "bytea")] // Dla PostgreSQL, typ binary
        public byte[] FileData { get; set; } // Plik PDF przechowywany jako byte[]

        public string Status { get; set; } // "Oczekujący", "Zatwierdzony", "Odrzucony"

        // Zatwierdzający
        public int Approver1ID { get; set; }
        public int Approver2ID { get; set; }
        public int Approver3ID { get; set; }

        public bool Approver1Approved { get; set; } = false; // Status zatwierdzenia
        public bool Approver2Approved { get; set; } = false;
        public bool Approver3Approved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Data utworzenia
    }
}
