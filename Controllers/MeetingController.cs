using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class MeetingController : Controller
    {
        private readonly IUnitOfWork<Meeting> _context;
        private readonly IUnitOfWork<Learner> _learnerContext;
        private readonly IUnitOfWork<Parent> _parentContext;



        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public MeetingController(IUnitOfWork<Meeting> context,
                               IUnitOfWork<Learner> learnerContext,
                               IUnitOfWork<Parent> parentContext,
                               IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

            _learnerContext = learnerContext;

            _parentContext = parentContext;

        }

        [HttpGet]
        public async Task<IActionResult> ViewAllMeeting()
        {
            return View(await _context.OnLoadItemsAsync());
        }

        [HttpGet]
        public IActionResult ScheduleMeeting()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleMeeting(Meeting meeting)
        {
            bool IsOk = false;

            if (ModelState.IsValid)
            {
                meeting.MeetingId = Utility.GenerateGuid();

                meeting.CreatedBy = Utility.loggedInUser;

                meeting.IsActive = true;

                meeting.CreatedOn = Utility.OnGetCurrentDateTime();

                var learners = await _learnerContext.OnLoadItemsAsync();

                IEnumerable<Learner> filterLearners = from n in learners

                                                      where n.Grade == meeting.Grade

                                                      select n;

                List<Learner> learnerList = filterLearners.ToList();

                List<Parent> parents = await _parentContext.OnLoadItemsAsync();

                for (int i = 0; i < learnerList.Count; i++)
                {
                    if (parents[i].LearnerId != null)
                    {
                        if (learnerList[i].LearnerId == parents[i].LearnerId)
                        {
                            //send email
                            var guardian = await _context.OnItemCreationAsync(meeting);

                            if (guardian != null)
                            {
                                int rc = await _context.ItemSaveAsync();

                                if (rc > 0)
                                {
                                    IsOk = true;

                                    break;
                                }
                            }
                        }
                    }
                }


            }
            else
            {
                _notify.Error("Error: Please fill in all the fields", 5);
            }

            if (IsOk)
            {
                _notify.Success("Meeting successfully scheduled", 5);
            }
            else
            {
                _notify.Error("Error: Unable to schedule meeting at this time", 5);
            }

            return View();
        }
    }
}
