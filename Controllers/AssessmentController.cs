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

namespace SchoolApp.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notify { get; }
        public AssessmentController(ApplicationDbContext context, INotyfService notify)
        {
            _context = context;

            _notify = notify;
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AssessmentList()
        {
              return View(await _context.Assessment.ToListAsync());
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> ScheduleAssessment()
        { 
            return View();
        }

        [Authorize]
        public IActionResult AssessmentAttempt()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleAssessment(Assessment assessment)
        {
            var learners = await _context.Learners.Where(x => x.Grade == assessment.Grade).ToListAsync();

            if(learners != null)
            {
                var subs = await _context.Subjects.Where(m => m.Subj == assessment.Subject).ToListAsync();

                if (subs != null)
                {
                    if (ModelState.IsValid)
                    {
                        assessment.CreatedBy = Utility.loggedInUser;

                        assessment.AssessmentId = Utility.GenerateGuid();

                        assessment.IsActive = true;

                        var emails = from n in learners

                                     where n.Email != null

                                     select n.Email;

                        if(emails != null)
                        {
                            foreach(string learnerMail in emails)
                            {
                                Utility.OnSendMailNotification(learnerMail, "Assessment Confirmation",
                                Utility.OnAssessmentSchedule(assessment.Grade.ToString(), assessment.Subject.ToString(), assessment.AssessmentDate.ToString("dddd, dd MMMM yyyy"), assessment.AssessmentTime.ToString("hh:mm"),
                                                             assessment.AssessmentType.ToString()),"Learner Assessment");
                                    
                            }

                            var assess = await _context.AddAsync(assessment);

                            if (assess != null)
                            {
                                int rc = await _context.SaveChangesAsync();

                                if (rc > 0)
                                {
                                    _notify.Success("Assessment scheduled successfully", 5);
                                }
                                else
                                {
                                    _notify.Error("Error: Unable to schedule assessment", 5);
                                }
                            }
                            else
                            {
                                _notify.Error("Error: Something went wrong", 5);

                            }

                        }

                    }
                    else
                    {
                        _notify.Error("Error: All fields are required!", 5);

                    }
                }
            }
            else
            {
                _notify.Error("Error: No learner enrolled for this subject under this grade!", 5);
            }
            
            return View();

        }
    
    }
}
