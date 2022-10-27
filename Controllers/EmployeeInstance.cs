using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    public class EmployeeInstance : Controller
    {
        private ApplicationDbContext _context;
        public EmployeeInstance(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> EmployeeInstaceModel(EmployeeViewModel model, Guid EmployeeId)
        {
            var onGetEmployee = await _context.Employees.ToListAsync();

            EmployeeViewModel viewModel = new EmployeeViewModel
            {
                EmployeeModel = _context.Employees.Where(m => m.EmployeeId == EmployeeId).FirstOrDefault(),

                EmployeeAddress = _context.Addresses.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault(),

                EmployeeQualifications = _context.Qualification.Where(x => x.EmployeeId == EmployeeId).ToList()
                
            };

            if (ModelState.IsValid)
            {
                //model
                _context.SaveChanges();
            }

            return View();
        }
    }
}
