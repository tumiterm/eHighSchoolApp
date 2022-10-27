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
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork<Address> _context;
        private readonly IUnitOfWork<Employee> _empContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public AddressController(IUnitOfWork<Address> context,
                               IWebHostEnvironment hostEnvironment, IUnitOfWork<Employee> empContext,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

            _empContext = empContext;

        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.OnLoadItemsAsync());
        }

        public async Task<IActionResult> AddUserAddress(Guid EmployeeId)
        {
            if(EmployeeId == Guid.Empty)
            {
                return NotFound();
            }

            Employee employee = await _empContext.OnLoadItemAsync(EmployeeId);

            if (employee is null)
            {
                return NotFound();
            }

            Address address = new Address
            {
                EmployeeId = employee.EmployeeId
            };

            ViewData["User"] = $"{employee.Title} {employee.Name} {employee.LastName}";

            return View(address);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserAddress(Address address)
        {
            if (ModelState.IsValid)
            {
                address.IsActive = true;

                address.CreatedBy = Utility.loggedInUser;

                address.CreatedOn = Utility.OnGetCurrentDateTime();

                address.AddressId = Utility.GenerateGuid();

                var addressObj = await _context.OnItemCreationAsync(address);

                if(addressObj != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Employee Address saved successfully", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save address", 5);
                    }

                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Something went wrong! [Model]", 5);

            }
            return RedirectToAction("AddUserQualification", "Qualification", new { EmployeeId = address.EmployeeId});
        }

    }
}
