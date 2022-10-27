using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{

    public class AttendanceController : Controller
    {
        private readonly IUnitOfWork<Attendance> _context;

        private readonly IUnitOfWork<Learner> _learnerContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public AttendanceController(IUnitOfWork<Attendance> context,
                                    IUnitOfWork<Learner> learnerContext,
                                    IWebHostEnvironment hostEnvironment,
                                    INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _learnerContext = learnerContext;

            _notify = notify;

        }
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> ComputeAttendance(Guid LearnerId)
        {
            if(LearnerId == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var loadLearner = await _learnerContext.OnLoadItemAsync(LearnerId);

            ViewData["LearnerId"] = loadLearner.LearnerId;

            ViewData["Detail"] = $"{loadLearner.Name} {loadLearner.LastName} <{loadLearner.Grade}>";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> ComputeAttendance(Attendance attendance)
        {
            if(attendance.Absent >= 0)
            {
                if (ModelState.IsValid)
                {
                    attendance.IsActive = true;

                    attendance.CreatedBy = Utility.loggedInUser;

                    attendance.CreatedOn = Utility.OnGetCurrentDateTime();

                    var learnerAttendace = await _context.OnItemCreationAsync(attendance);

                    if (learnerAttendace != null)
                    {
                        int rc = await _context.ItemSaveAsync();

                        if (rc > 0)
                        {
                            _notify.Success("Learner attendance captured successfully", 5);
                        }
                        else
                        {
                            _notify.Error("Error: Unable to capture learner attendance", 5);
                        }
                    }
                    else
                    {
                        _notify.Error("Error: Unable to capture learner attendance!", 5);

                    }


                }
                else
                {
                    _notify.Error("Error: Some fields are missing!", 5);

                }
                return View();
            }
            else
            {
                _notify.Error("Error: Days cant be a negative number", 5);
            }
            return View();
           
        }

        [Authorize]
        public IActionResult OnComputedAttendance()
        {
            return View();
        }
    }
}
