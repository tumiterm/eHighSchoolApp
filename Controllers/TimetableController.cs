using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class TimetableController : Controller
    {

        private readonly IUnitOfWork<TimeTable> _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public INotyfService _notify { get; }
        public TimetableController(IUnitOfWork<TimeTable> context,
            IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _notify = notify;

            _hostEnvironment = hostEnvironment;

        }
        [HttpGet]
        public IActionResult CreateTimeTable()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTimeTable(TimeTable timeTable)
        {
            timeTable.Id = Utility.GenerateGuid();

            timeTable.IsActive = true;

            timeTable.CreatedBy = Utility.loggedInUser;

            timeTable.CreatedOn = Utility.OnGetCurrentDateTime();

            timeTable.Attachment = timeTable.AttachFile.FileName;

            if (ModelState.IsValid)
            {
                AttachmentUploader(timeTable);

                var ttable = await _context.OnItemCreationAsync(timeTable);

                if(ttable != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        
                        _notify.Success("Timetable successfully added", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Timetable NOT added!",5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: All fields are required!", 5);

            }

            return View();
        }

        public async void AttachmentUploader(TimeTable timeTable)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(timeTable.AttachFile.FileName);

            string extension = Path.GetExtension(timeTable.AttachFile.FileName);

            timeTable.Attachment = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/Timetable/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await timeTable.AttachFile.CopyToAsync(fileStream);
            }
        }

        public async Task<IActionResult> AttachmentDownload(string filename)
        {
            if (filename == null)

                return Content("Sorry NO Attachment Added by Parent");


            var path1 = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot");

            string folder = path1 + @"\Timetable\" + filename;

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
