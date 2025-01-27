using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace VicUniIndustryProject2025LiveKit
{
    public class ChatDbController : Controller
    {
        private readonly ChatDbContext _context;

        public ChatDbController(ChatDbContext context)
        {
            _context = context;
        }

        //GET THE DATABASE FOR EACH 
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            // Fetch the list of employees from the database asynchronously
            var employees = await _context.Employees.ToListAsync();

            // Return the employees in a 200 OK response
            return Ok(employees);
        }


    }
}
