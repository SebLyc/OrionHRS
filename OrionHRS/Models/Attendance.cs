using System;

namespace OrionHRS.Models
{
    public class Attendance
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Day { get; set; }
        public string Status { get; set; }
        public string ConfirmedBy { get; set; }
        public string SubmissionStatus { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }

        // Relacja do Employee
        public Employee Employee { get; set; }
    }
}
