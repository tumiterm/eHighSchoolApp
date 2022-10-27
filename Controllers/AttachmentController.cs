using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        private readonly IUnitOfWork<Attachment> _context;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public AttachmentController(IUnitOfWork<Attachment> context,
                               IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

        }

        [HttpGet]
        public async Task<IActionResult> AddUserAttachment(Guid EmployeeId)
        {
            if(EmployeeId == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");

            }
            var user = await _context.OnLoadItemAsync(EmployeeId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserAttachment(Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                attachment.IsActive = true;

                attachment.CreatedOn = Utility.OnGetCurrentDateTime();

                attachment.CreatedBy = Utility.loggedInUser;

                var docs = await _context.OnItemCreationAsync(attachment);

                if (docs != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Attachment saved successfully", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save attachment!",5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong", 5);
                }
            }
            else
            {
                _notify.Error("Error: Something went wrong [Model]", 5);
            }

            return View();
        }
    }
}
