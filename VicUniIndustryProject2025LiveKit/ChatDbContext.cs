using Microsoft.EntityFrameworkCore;
using VicUniIndustryProject2025LiveKit.EditableModels;

namespace VicUniIndustryProject2025LiveKit
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext()
        {
        }
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OnSite> OnSites { get; set; }
    }
}
