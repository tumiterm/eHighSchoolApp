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
    public class LearnerCompletionController : Controller
    {

        private readonly IUnitOfWork<Address> _addrContext;
        private readonly IUnitOfWork<Parent> _parContext;
        private readonly IUnitOfWork<Subject> _subContext;
        private readonly IUnitOfWork<School> _schContext;
        private readonly IUnitOfWork<Learner> _learContext;
        private ApplicationDbContext _dbContext;
        public INotyfService _notify { get; }
        public LearnerCompletionController(IUnitOfWork<Address> addrContext, IUnitOfWork<Parent> parContext,
                                           IUnitOfWork<Subject> subContext, IUnitOfWork<School> schContext,
                                           INotyfService notify, IUnitOfWork<Learner> learContext, ApplicationDbContext dbContext)
        {
            _addrContext = addrContext;

            _parContext = parContext;

            _schContext = schContext;

            _subContext = subContext;

            _learContext = learContext;

            _notify = notify;

            _dbContext = dbContext;

        }

        public async Task<IActionResult> OnEnrollmentCompletion(Guid LearnerId)
        {

            if(LearnerId == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }


            LearnerViewModel viewModel = new LearnerViewModel
            {
                LearnerModel = await _learContext.OnLoadItemAsync(LearnerId),

                LearnerParent = await _dbContext.Parents.Where(m => m.LearnerId == LearnerId).FirstOrDefaultAsync(),

                LearnerSchool = await _schContext.OnLoadItemsAsync(),

                LearnerSubjects =  _dbContext.Subjects.Where(x => x.LearnerId == LearnerId).ToList()

            };

            string details = $"{viewModel.LearnerModel.Name} {viewModel.LearnerModel.LastName} | {viewModel.LearnerModel.Grade}";

            ViewData["Details"] = details;

            ViewData["Subjects"] = viewModel.LearnerSubjects.ToList();

            Utility.OnSendMailNotification(viewModel.LearnerModel.Email, "Confirmation of Enrollment",
                                            Utility.OnConfirmationMessage(details, viewModel.LearnerModel.LearnerReference, viewModel.LearnerModel.RSAIDNumber,
                                            viewModel.LearnerModel.Grade.ToString()),"Online Applications");
            return View(viewModel);
        }
    }
}
