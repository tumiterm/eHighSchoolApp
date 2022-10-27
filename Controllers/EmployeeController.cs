using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork<Employee> _context;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public EmployeeController(IUnitOfWork<Employee> context,
                               IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.OnLoadItemsAsync());
        }
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee empObj = new Employee();

                employee.IDPassportFile += employee.IDPassportFormFile.FileName;

                employee.EmployeeId = Utility.GenerateGuid();

                employee.CreatedBy = Utility.loggedInUser;

                employee.CreatedOn = Utility.OnGetCurrentDateTime();

                employee.IsActive = true;

                if (employee.Nationality == eNums.eNationality.SouthAfrica)
                {
                    if (!_context.DoesEntityExist<Employee>(m => m.RSAIdNumber == employee.RSAIdNumber))
                    {
                        empObj = await _context.OnItemCreationAsync(employee);
                    }
                    else
                    {
                        _notify.Error($"Error: Employee with ID: {employee.RSAIdNumber} already exist!");
                    }
                }
                else
                {
                    if (!_context.DoesEntityExist<Employee>(m => m.PassportNum == employee.PassportNum))
                    {
                        empObj = await _context.OnItemCreationAsync(employee);
                    }
                    else
                    {
                        _notify.Error($"Error: Employee with Passport Number: {employee.PassportNum} already exist!");

                    }

                }

                if (empObj != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if (rc > 0)
                    {
                        _notify.Success("Employee Registered Successfully", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to register employee!", 5);
                    }
                }

                return RedirectToAction("AddUserAddress","Address",new {EmployeeId = employee.EmployeeId});
            }

            return View(employee);
        }

        

    }

}
