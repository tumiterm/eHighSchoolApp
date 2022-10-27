using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    public class RSVPController : Controller
    {

        private readonly IUnitOfWork<RSVP> _context;
        public INotyfService _notify { get; }
        public RSVPController(IUnitOfWork<RSVP> context,
                               INotyfService notify)
        {
            _context = context;

            _notify = notify;
        }

        [HttpGet]
        public async Task<IActionResult> AddRSVP(Guid MeetingId)
        {
            if(MeetingId == Guid.Empty)
            {
                return RedirectToAction("NotFound","Global");
            }

            var meeting = await _context.OnLoadItemAsync(MeetingId);

            ViewBag.MeetingId = MeetingId;

            return View(meeting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRSVP(RSVP rsvp)
        {
            rsvp.Id = Utility.GenerateGuid();

            rsvp.IsActive = true;

            rsvp.CreatedBy = Utility.loggedInUser;

            rsvp.CreatedOn = Utility.OnGetCurrentDateTime();

            if (ModelState.IsValid)
            {
                var meet = await _context.OnItemCreationAsync(rsvp);

                if (meet != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if (rc > 0)
                    {
                        _notify.Success("User selection successfully captured", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save selection!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Unable something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Please choose an option!", 5);

            }

            return RedirectToAction("ViewAllMeeting", "Meeting");
        }
    }
}
