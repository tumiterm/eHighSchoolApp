using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public class QualificationController : Controller
    {
        private readonly IUnitOfWork<Qualification> _context;
        private readonly IUnitOfWork<Employee> _empContext;


        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public QualificationController(IUnitOfWork<Qualification> context,
                               IWebHostEnvironment hostEnvironment, IUnitOfWork<Employee> empContext,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _empContext = empContext;

            _notify = notify;

        }
        public async Task<IActionResult> Index()
        {
              return View(await _context.OnLoadItemsAsync());
        }

        public async Task<IActionResult> AddUserQualification(Guid EmployeeId)
        {
            if(EmployeeId == Guid.Empty)
            {
                return NotFound();
            }

            var employee = await _empContext.OnLoadItemAsync(EmployeeId);

            if(employee is null)
            {
                return NotFound();
            }

            var qualifications = await _context.OnLoadItemsAsync();

            var qualObj = from n in qualifications

                          where n.EmployeeId == employee.EmployeeId

                          select n;

            dynamic subModel = new ExpandoObject();

            subModel.SujectInstance = qualObj.ToList();

            ViewData["EmployeeId"] = employee.EmployeeId;

            ViewData["details"] = $"{employee.Title} {employee.LastName} {employee.Name}";

            return View(subModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserQualification(Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                qualification.QualificationId = Utility.GenerateGuid();

                qualification.CreatedBy = Utility.loggedInUser;

                qualification.CreatedOn = Utility.OnGetCurrentDateTime();

                qualification.IsActive = true;

                var qual = await _context.OnItemCreationAsync(qualification);

                if(qual != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Qualification successfully added", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to add qualification", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: something went wrong!");
                }
            }
            else
            {
                _notify.Error("Error: something went wrong! [Model]");

            }

            return RedirectToAction(nameof(AddUserQualification),new { EmployeeId = qualification.EmployeeId});
        }

        // GET: Qualification/Edit/5
        public async Task<IActionResult> OnAddUserQualification(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

           //// var qualification = await _context.Qualification.FindAsync(id);
           // if (qualification == null)
           // {
           //     return NotFound();
           // }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnAddUserQualification(Guid id, Qualification qualification)
        {
            if (id != qualification.QualificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!QualificationExists(qualification.QualificationId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(qualification);
        }

        //// GET: Qualification/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.Qualification == null)
        //    {
        //        return NotFound();
        //    }

        //    var qualification = await _context.Qualification
        //        .FirstOrDefaultAsync(m => m.QualificationId == id);
        //    if (qualification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(qualification);
        //}

        //// POST: Qualification/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    if (_context.Qualification == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Qualification'  is null.");
        //    }
        //    var qualification = await _context.Qualification.FindAsync(id);
        //    if (qualification != null)
        //    {
        //        _context.Qualification.Remove(qualification);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool QualificationExists(Guid id)
        //{
        //  return _context.Qualification.Any(e => e.QualificationId == id);
        //}
    }
}
