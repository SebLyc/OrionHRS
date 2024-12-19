using Microsoft.EntityFrameworkCore;

namespace OrionHRS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet dla modeli
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Document> Documents { get; set; }


    }
}
