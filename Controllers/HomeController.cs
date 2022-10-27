using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.ViewModels;
using System.Diagnostics;

namespace SchoolApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;

            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewData["learners"] = await _context.Learners.CountAsync();

            ViewData["users"] = await _context.Users.CountAsync();

            ViewData["assessments"] = await _context.Assessment.CountAsync();

            ViewData["femaleLearners"] = await _context.Learners.Where(m => m.Gender == eNums.eGender.Female).CountAsync();

            ViewData["maleLearners"] = await _context.Learners.Where(m => m.Gender == eNums.eGender.Male).CountAsync();

            ViewData["Messages"] = await _context.Messages.CountAsync();

            ViewData["Emps"] = await _context.Employees.CountAsync();

            ViewData["Meet"] = await _context.Meeting.CountAsync();


            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Learner,Admin")]
        public async Task<IActionResult> LearnerBoard()
        {
            LearnerViewModel learner = new LearnerViewModel
            {
                Assessments = _context.Assessment.ToList(),

                Messages = _context.Messages.ToList(),

                Attendance = _context.Attendances.ToList(),

                TimeTables = _context.EventTimeTable.ToList()

            };

            for (int i = 0; i < learner.Assessments.Count; i++)
            {
                ViewData["AssVenue"] = learner.Assessments[i].Classroom;
                ViewData["AssDate"] = learner.Assessments[i].AssessmentDate;
                ViewData["AssTime"] = learner.Assessments[i].AssessmentTime;
                ViewData["AssType"] = learner.Assessments[i].AssessmentType;
            }

            for (int i = 0; i < learner.Messages.Count; i++)
            {

                ViewData["Messages"] = learner.Messages[i].MessageInfo;
                ViewData["Subject"] = learner.Messages[i].Subject;
                ViewData["Date"] = learner.Messages[i].CreatedOn;
                ViewData["By"] = learner.Messages[i].CreatedBy;

            }

            for (int i = 0; i < learner.Attendance.Count; i++)
            {
                ViewData["Absent"] = learner.Attendance[i].Absent;
                ViewData["Comment"] = learner.Attendance[i].Comment;
                ViewData["From"] = learner.Attendance[i].From;
                ViewData["Till"] = learner.Attendance[i].Till;
            }

            for (int m = 0; m < learner.TimeTables.Count; m++)
            {
                ViewData["tTitle"] = learner.TimeTables[m].Title;
                ViewData["tCreate"] = learner.TimeTables[m].CreatedBy;
                ViewData["tDate"] = learner.TimeTables[m].CreatedOn;
                ViewData["tGrade"] = learner.TimeTables[m].Grade;

            }

            return View(await _context.EventTimeTable.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Parent,Admin")]

        public async Task<IActionResult> ParentLandingPage()
        {

            LearnerParentViewModel parentViewModel = new LearnerParentViewModel
            {
                Meetings = await _context.Meeting.ToListAsync(),

                Assessments = await _context.Assessment.ToListAsync(),

                Attendances = await _context.Attendances.ToListAsync()
            };

            for (int i = 0; i < parentViewModel.Meetings.Count; i++)
            {
                ViewData["Date"] = parentViewModel.Meetings[i].Date;
                ViewData["Message"] = parentViewModel.Meetings[i].Message;
                ViewData["Time"] = parentViewModel.Meetings[i].Time;
                ViewData["Venue"] = parentViewModel.Meetings[i].Venue;
            }

            for (int i = 0; i < parentViewModel.Assessments.Count; i++)
            {
                ViewData["AssDate"] = parentViewModel.Assessments[i].AssessmentDate;
                ViewData["Time"] = parentViewModel.Assessments[i].AssessmentTime;
                ViewData["Type"] = parentViewModel.Assessments[i].AssessmentType ;
                ViewData["Venue"] = parentViewModel.Assessments[i].Classroom;
            }

            for (int i = 0; i < parentViewModel.Attendances.Count; i++)
            {
                ViewData["Absent"] = parentViewModel.Attendances[i].Absent;
                ViewData["Comment"] = parentViewModel.Attendances[i].Comment;
                ViewData["From"] = parentViewModel.Attendances[i].From;
                ViewData["Till"] = parentViewModel.Attendances[i].Till;
            }


            return View(parentViewModel);
        }

        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> TeacherLandingPage()
        {
            LearnerTeacherViewModel viewModel = new LearnerTeacherViewModel
            {
                Assessments = await _context.Assessment.ToListAsync(),

                Learners = await _context.Learners.ToListAsync(),

                Meetings = await _context.Meeting.ToListAsync(),

                ParentMessages = await _context.ParentMessages.ToListAsync()
            };

            for (int i = 0; i < viewModel.ParentMessages.Count; i++)
            {
                ViewData["Message"] = viewModel.ParentMessages[i].Message;
                ViewData["MType"] = viewModel.ParentMessages[i].MessageType;
                ViewData["Learner"] = viewModel.ParentMessages[i].Learner;
                ViewData["Date"] = viewModel.ParentMessages[i].CreatedOn;
                ViewData["By"] = viewModel.ParentMessages[i].CreatedBy;

            }

            for (int i = 0; i < viewModel.Assessments.Count; i++)
            {
                ViewData["Subj"] = viewModel.Assessments[i].Subject;
                ViewData["AssDate"] = viewModel.Assessments[i].AssessmentDate;
                ViewData["Time"] = viewModel.Assessments[i].AssessmentTime;
                ViewData["Type"] = viewModel.Assessments[i].AssessmentType;
                ViewData["Venue"] = viewModel.Assessments[i].Classroom;
                ViewData["Subj"] = viewModel.Assessments[i].Subject;

            }

            for (int i = 0; i < viewModel.Meetings.Count; i++)
            {
                ViewData["MeetingDate"] = viewModel.Meetings[i].Date;
                ViewData["CreatedBy"] = viewModel.Meetings[i].CreatedBy;
                ViewData["MeetingTime"] = viewModel.Meetings[i].Time;
                ViewData["Urgency"] = viewModel.Meetings[i].Urgency;
                ViewData["Venue"] = viewModel.Meetings[i].Venue;

            }

            
            return View(await _context.ParentMessages.ToListAsync());
        }
    }
}