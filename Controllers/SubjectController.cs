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
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork<Subject> _context;
        private readonly IUnitOfWork<Learner> _learnerContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public SubjectController(IUnitOfWork<Subject> context, IUnitOfWork<Learner> learnerContext,
                                IWebHostEnvironment hostEnvironment,
                                INotyfService notify)
        {
            _learnerContext = learnerContext;

            _context = context;

            _hostEnvironment = hostEnvironment;

            _hostEnvironment = hostEnvironment;

            _notify = notify;

        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.OnLoadItemsAsync());
        }
        public async Task<IActionResult> SubjectAddition(Guid LearnerId)
        {
            var learner = await _learnerContext.OnLoadItemAsync(LearnerId);

            if (learner is null)
            {
                return NotFound();
            }

            dynamic dynamicObj = new ExpandoObject();

            var subjects = await _context.OnLoadItemsAsync();

            var filterSubj = from n in subjects

                             where n.LearnerId == LearnerId

                             select n;

            dynamicObj.SubjList = filterSubj.ToList();

            ViewData["learner"] = $"{learner.Name} {learner.LastName} | {learner.Grade}";

            ViewData["LearnerId"] = LearnerId;

            return View(dynamicObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectAddition(Subject subject)
        {
            const int MAX_SUBJECT = 6;

            string date = Utility.OnGetCurrentDateTime();

            subject.Date = date;

            if (ModelState.IsValid)
            {

                if (await OnSubjectCount(subject.LearnerId) <= MAX_SUBJECT)
                {
                    subject.Id = Utility.GenerateGuid();

                    subject.Date = Utility.OnGetCurrentDateTime();

                    subject.IsActive = true;

                    if (!await OnIsSubjectAdded(subject.Subj.ToString(), subject.LearnerId))
                    {
                        if (await _context.OnItemCreationAsync(subject) != null)
                        {
                            if (await _context.ItemSaveAsync() > 0)
                            {
                                _notify.Success("Subject Added successfully", 5);
                            }
                            else
                            {
                                _notify.Error("Error: Subject could NOT be added!", 5);
                            }
                        }
                        else
                        {
                            _notify.Error("Error: Something went wrong!", 5);

                        }
                    }
                    else
                    {
                        _notify.Error("Error: Subject already Chosen!", 5);

                    }
                }
                else
                {
                    _notify.Error("Error: Cannot chose more than 7 subjects", 5);

                    _notify.Warning("Subject selection is completed for user", 5);

                    return RedirectToAction("OnEnrollmentCompletion", "LearnerCompletion", new { LearnerId = subject.LearnerId });

                }
            }
            else
            {
                _notify.Error("Error: You have NOT chosen any subjects!!", 5);
            }

            if (!subject.IsDone)
            {
                return RedirectToAction(nameof(SubjectAddition), new { LearnerId = subject.LearnerId });

            }

            return RedirectToAction("OnEnrollmentCompletion", "LearnerCompletionController", new { LearnerId = subject.LearnerId });

        }

        public async Task<IActionResult> RemoveSubject(Guid Id)
        {
            if(Id == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var subject = await _context.OnLoadItemAsync(Id);

            _context.OnRemoveItemAsync(Id);

            return RedirectToAction(nameof(SubjectAddition), new { LearnerId = subject.LearnerId });
        }


        private async Task<int> OnSubjectCount(Guid LearnerId)
        {
            var sub = await _context.OnLoadItemsAsync();

            var subFilter = from a in sub

                            where a.LearnerId == LearnerId

                            select a;

            return subFilter.Count();
        }
        private async Task<bool> OnIsSubjectAdded(string learnerSubject, Guid LearnerId)
        {
            bool IsAdded = false;

            var sub = await _context.OnLoadItemsAsync();

            var learnerSubjects = from n in sub

                                  where n.LearnerId == LearnerId

                                  && n.Subj.ToString().Equals(learnerSubject)

                                  select n;

            if(learnerSubjects.Count() > 0)
            {
                IsAdded = true;
            }

            return IsAdded;
        }
    }
}
