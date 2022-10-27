using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolApp.Data;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class ParentNotificationController : Controller
    {
        private readonly IUnitOfWork<Employee> _empContext;
        private readonly IUnitOfWork<ParentMessage> _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public INotyfService _notify { get; }
        public ParentNotificationController(IUnitOfWork<Employee> empContext,
                                            IUnitOfWork<ParentMessage> context,
                                            IWebHostEnvironment hostEnvironment,
                                            INotyfService notify)
        {
            _empContext = empContext;

            _notify = notify;

            _context = context;

            _hostEnvironment = hostEnvironment;

        }

        [HttpGet]
        public async Task<IActionResult> OnSendTeacherMessage()
        {
            var teachers = await _empContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> teacher = from s in teachers

                                                      select new SelectListItem
                                                      {
                                                          Value = s.EmployeeId.ToString(),

                                                          Text = $"{s.Title} {s.Name} {s.LastName} | ({s.Grade})"
                                                      };

            ViewBag.TeacherId = new SelectList(teacher, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnSendTeacherMessage(ParentMessage parentMessage)
        {
            parentMessage.MessageId = Utility.GenerateGuid();

            parentMessage.IsActive = true;

            parentMessage.CreatedBy = Utility.loggedInUser;

            parentMessage.CreatedOn = Utility.OnGetCurrentDateTime();

            parentMessage.Attachment = parentMessage.AttachmentFile.FileName;

            if (ModelState.IsValid)
            {
                var teachers = await _empContext.OnLoadItemsAsync();

                var filterTeachers = from n in teachers

                                     where n.EmployeeType == eNums.eEmployeeType.Educator

                                     select n;

                var messageObj = await _context.OnItemCreationAsync(parentMessage);

                if(messageObj != null)
                {
                    AttachmentUploader(parentMessage);

                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Message successfully sent to teacher", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to send message!", 5);
                    }

                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }

            }
            else
            {
                _notify.Error("Error: Please fill in all the fields!", 5);
            }

            return View();
        }

        [NonAction]
        public async void AttachmentUploader(ParentMessage parent)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(parent.AttachmentFile.FileName);

            string extension = Path.GetExtension(parent.AttachmentFile.FileName);

            parent.Attachment = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/Attachment/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await parent.AttachmentFile.CopyToAsync(fileStream);
            }
        }

        public async Task<IActionResult> AttachmentDownload(string filename)
        {
            if (filename == null)

                return Content("Sorry NO Attachment Added by Parent");


            var path1 = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot");

            string folder = path1 + @"\Attachment\" + filename;

            var memory = new MemoryStream();

            using (var stream = new FileStream(folder, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, Utility.GetContentType(folder), Path.GetFileName(folder));
        }
    }

}
