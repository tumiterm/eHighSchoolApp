using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{

    [Authorize(Roles = "Admin")]
    public class SchoolController : Controller
    {
        private readonly IUnitOfWork<School> _context;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public SchoolController(IUnitOfWork<School> context,
                                IWebHostEnvironment hostEnvironment,
                                INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

        }
        public IActionResult RegisterSchool()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSchool(School school)
        {
            if (ModelState.IsValid)
            {
                school.SchoolId = Utility.GenerateGuid();

                school.IsActive = true;

                school.CreatedBy = Utility.loggedInUser;

                school.CreatedOn = Utility.OnGetCurrentDateTime();

                var schoolObj = await _context.OnItemCreationAsync(school);

                int rc = await _context.ItemSaveAsync();

                if(schoolObj != null)
                {
                    if(rc > 0)
                    {
                        _notify.Success("School successfully added", 5);
                    }
                    else
                    {
                        _notify.Error("Error: School NOT Added", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: something went wrong - [Create]", 5);
                }
            }
            else
            {
                _notify.Error("Error: something went wrong - [Model]", 5);

            }
            return View();
        }
    }
}
