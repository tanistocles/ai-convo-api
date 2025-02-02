using Microsoft.EntityFrameworkCore;
using VicUniIndustryProject2025LiveKit.EditableModels;
using VicUniIndustryProject2025LiveKit.Results;

namespace VicUniIndustryProject2025LiveKit
{
    public class ChatService
    {
        private readonly ChatDbContext dbContext;

        //Need to put this into the database
        List<string> approvedContractorCompanies = new List<string>
        {
            "Acme Plumbing",
            "XYZ Electrical",
            "Best Builders"
        };

        public ChatService(ChatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SeedDatabaseAsync()
        {
            var employees = new List<Employee>
            {
                new Employee(4, "Tan", "tan@gmail.com"),
                //new Employee(2, "Bob", "bob@example.com", 2345),
                //new Employee(1, "Alice", "alice@example.com", 1234),
                //new Employee(3, "Charlie", "charlie@example.com", 3456)
            };

            if (!await dbContext.Employees.AnyAsync())
            {
                dbContext.Employees.AddRange(employees);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<Product>> GetAllProductAsync()
        {
            return await dbContext.Products.ToListAsync();
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await dbContext.Employees.ToListAsync();
        }

        public async Task<List<Visitor>> GetAllVisitorsAsync()
        {
            return await dbContext.Visitors.ToListAsync();
        }

        public async Task<List<OnSite>> GetAllOnSiteAsync()
        {
            return await dbContext.OnSites.ToListAsync();
        }

        public async Task<List<Visitor>> GetVisitorsOnSiteAsync()
        {
            return await dbContext.Visitors.Where(v => v.IsOnSite).ToListAsync();
        }

        public async Task<VisitorArriveMeetingResult> VisitorArriveMeetingAsync(VisitorArriveMeetingRequest request)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Name.ToLower().Equals(request.MeetingWith.ToLower()));

            if (employee == null)
            {
                return new VisitorArriveMeetingResult()
                {
                    Approved = false,
                    Message = $"Employee '{request.MeetingWith}' does not exist."
                };
            }

            // Create a new visitor (contractor)
            var visitor = new Visitor(
                //Id: nextVisitorId++,
                Name: request.VisitorName,
                ArrivalTime: DateTime.UtcNow,
                IsOnSite: true,
                Reason: "Meeting",
                MeetingWith: employee.Name,
                ContractorCompany: null,
                DepartureTime: null
            );

            dbContext.Visitors.Add(visitor);

            await dbContext.SaveChangesAsync();

            var message = $"Visitor '{request.VisitorName}' has arrived for meeting with '{employee.Name}'. Notification sent.";

            // Notify reception logic (pseudo)
            return new VisitorArriveMeetingResult
            {
                Approved = true,
                Message = message
            };
        }

        public async Task<VisitorArriveCourierResult> VisitorArriveCourierAsync(VisitorArriveCourierRequest request)
        {
            // Create a new visitor (courier)
            var visitor = new Visitor(
                //Id: nextVisitorId++,
                Name: request.CourierName,
                ArrivalTime: DateTime.UtcNow,
                IsOnSite: true,
                Reason: "Courier",
                MeetingWith: null,
                ContractorCompany: null,
                DepartureTime: null
            );

            dbContext.Visitors.Add(visitor);

            await dbContext.SaveChangesAsync();


            // In a real system, you'd notify reception via email/alert, etc.
            var message = $"Courier '{request.CourierName}' has arrived. Reception notified. " +
                          "Please leave the parcel at the designated location.";

            // Notify reception logic (pseudo)
            return new VisitorArriveCourierResult
            {
                Approved = true,
                Message = message
            };
        }


        public async Task<VisitorArriveContractorResult> VisitorArriveContractorAsync(VisitorArriveContractorRequest request)
        {
            // Check if the contractor's company is in the approved list
            var isApproved = approvedContractorCompanies.Any(c =>
                c.ToLower().Equals(request.Company.ToLower()));

            if (!isApproved)
            {
                return new VisitorArriveContractorResult
                {
                    Approved = false,
                    Message = $"Contractor's company '{request.Company}' is not approved."
                };
            }

            // Create a new visitor (contractor)
            var visitor = new Visitor(
                //Id: nextVisitorId++,
                Name: request.VisitorName,
                ArrivalTime: DateTime.UtcNow,
                IsOnSite: true,
                Reason: "Contractor",
                MeetingWith: null,
                ContractorCompany: request.Company,
                DepartureTime: null
            );

            dbContext.Visitors.Add(visitor);

            await dbContext.SaveChangesAsync();

            // Notify reception logic (pseudo)
            var message = $"Contractor '{request.VisitorName}' from '{request.Company}' has arrived. Reception notified.";
            return new VisitorArriveContractorResult
            {
                Approved = true,
                Message = message
            };
        }


        public async Task<VisitorSignOutResult> VisitorSignOutAsync(VisitorSignOutRequest request)
        {
            var visitor = await this.dbContext.Visitors.FirstOrDefaultAsync(v => v.Id == request.VisitorId);

            if (visitor == null)
            {
                return new VisitorSignOutResult
                {
                    Approved = false,
                    Message = $"Visitor with ID '{request.VisitorId}' does not exists."
                };
            }

            if (!visitor.IsOnSite)
            {
                return new VisitorSignOutResult
                {
                    Approved = false,
                    Message = $"Visitor '{visitor.Name}' is not on site."
                };
            }

            visitor.IsOnSite = false;
            visitor.DepartureTime = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            // Notify reception logic (pseudo)
            var message = $"Visitor '{visitor.Name}' has left";


            return new VisitorSignOutResult
            {
                Approved = true,
                Message = message
            };
        }
    }
}
