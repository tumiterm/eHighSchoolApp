using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;
using SchoolApp.ViewModels;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class LearnerController : Controller
    {
        private readonly IUnitOfWork<Learner> _context;
        private ApplicationDbContext _dbContext;


        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public LearnerController(IUnitOfWork<Learner> context,
                                IWebHostEnvironment hostEnvironment,
                                ApplicationDbContext dbContext,

                                INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

            _dbContext = dbContext;

        }

       

        public async Task<IActionResult> OnViewLearnerSubjects(string IDPassport)
        {
            if (string.IsNullOrEmpty(IDPassport))
            {
                _notify.Error("Error: ID Error");
            }

            var learner = await _dbContext.Learners.FirstOrDefaultAsync(m => m.RSAIDNumber == IDPassport);

            var subjects = await _dbContext.Subjects.Where(x => x.LearnerId == learner.LearnerId).ToListAsync();

            if(subjects is null)
            {
                _notify.Error("Error: ID Error");

            }

            ViewData["Learner"] = $"{learner.Name} {learner.LastName} <{learner.Grade}>";

            ViewData["Subjects"] = subjects;

            return View(subjects);

        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnViewLearnerSubjects(Learner learner)
        {
            if (ModelState.IsValid)
            {

            }

            return View();
        }
        public async Task<IActionResult> EnrolledLearners()
        {
            return View(await _context.OnLoadItemsAsync());
        }

        [HttpGet]
        public IActionResult LearnerEnrollment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LearnerEnrollment(Learner learner)
        {
            if (ModelState.IsValid)
            {
                learner.LearnerId = Utility.GenerateGuid();

                learner.LearnerReference = $"Learn{DateTime.Now.Year}{Utility.RandomStringGenerator(5)}";

                learner.CreatedBy = Utility.loggedInUser;

                learner.CreatedOn = Utility.OnGetCurrentDateTime();

                learner.IsActive = true;

                IDPassportUploader(learner);

                ReportCardUpload(learner);

                var addLearner = _context.OnItemCreationAsync(learner);

                int rc = await _context.ItemSaveAsync();

                if (rc > 0)
                {
                    _notify.Success($"Learner {learner.Name} {learner.LastName} successfully enrolled", 5);
                }
                else
                {
                    _notify.Error("Error: Learner NOT enrolled!",5);
                }
            }
            else
            {
                _notify.Error("Error: Something went wrong [Model]",5);
            }

            return RedirectToAction("OnLearnerEnrolled","Learner", new {LearnerId = learner.LearnerId});
        }
        public async Task<IActionResult> OnLearnerEnrolled(Guid LearnerId)
        {
            if(LearnerId == Guid.Empty)
            {
                return NotFound();
            }

            return View(await _context.OnLoadItemAsync(LearnerId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnLearnerEnrolled(Learner learner)
        {
            if (ModelState.IsValid)
            {
                var onGetLearners = await _context.OnModifyItemAsync(learner);

                if(learner != null)
                {
                    _notify.Success("Learner saved successfully",5);
                }
                else
                {
                    _notify.Error("Error: Learner NOT saved!",5);
                }
            }
            else
            {
                _notify.Error("Error: Something went wrong [Mode]",5);
            }
            return View();
        }
        public async void ReportCardUpload(Learner learner)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(learner.ReportCardFile.FileName);

            string extension = Path.GetExtension(learner.ReportCardFile.FileName);

            learner.ReportCard = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/ReportCard/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await learner.ReportCardFile.CopyToAsync(fileStream);
            }
        }
        public async void IDPassportUploader(Learner learner)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(learner.IDPassportFile.FileName);

            string extension = Path.GetExtension(learner.IDPassportFile.FileName);

            learner.IDPassport = fileName = fileName + Utility.GenerateGuid() + extension;

            string path = Path.Combine(wwwRootPath + "/ID/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await learner.IDPassportFile.CopyToAsync(fileStream);
            }
        }

    }
}
