using Microsoft.EntityFrameworkCore;

namespace VicUniIndustryProject2025LiveKit
{
    public class ChatDbContext: DbContext
    {
        public ChatDbContext()
        {
        }
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //public DbSet<VisitorArriveContractorRequest> VisitorArriveContractorRequests { get; set; }
        //public DbSet<VisitorArriveCourierRequest> VisitorArriveCourierRequests { get; set; }
        //public DbSet<VisitorArriveMeetingRequest> VisitorArriveMeetingRequests { get; set; }
        //public DbSet <VisitorSignOutRequest> VisitorSignOutRequests { get; set; }
    }
}
