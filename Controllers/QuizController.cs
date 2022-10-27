using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.ViewModels;
using System.Dynamic;

namespace SchoolApp.Controllers
{
    public class QuizController : Controller
    {
        public INotyfService _notify { get; }
        private ApplicationDbContext _context;
        public QuizController(ApplicationDbContext context, INotyfService notify)
        {
            _context = context;

            _notify = notify;

        }
        public IActionResult GenerateQuiz()
        {
            dynamic dynamicObj = new ExpandoObject();

            var quizzes = _context.Quiz.ToList();

            dynamicObj.QuizList = quizzes.ToList();

            return View(dynamicObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateQuiz(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                quiz.QuizId = Utility.GenerateGuid();

                quiz.IsActive = true;

                quiz.CreatedBy = Utility.loggedInUser;

                quiz.CreatedOn = Utility.OnGetCurrentDateTime();

                var createQuiz = await _context.Quiz.AddAsync(quiz);

                if (createQuiz != null)
                {
                    int rc = await _context.SaveChangesAsync();

                    if (rc > 0)
                    {
                        _notify.Success("Quiz successfully created", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to create quiz!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);

                }
            }
            else
            {
                _notify.Error("Error: Some fields are missing!", 5);
            }

            return RedirectToAction(nameof(GenerateQuiz));
        }

        [HttpGet]
        public async Task<IActionResult> QuizQuestions(Guid AssociativeKey)
        {
            if (AssociativeKey == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var quiz = await _context.Quiz.FirstOrDefaultAsync(m => m.QuizId == AssociativeKey);

            ViewData["QDetails"] = $"{quiz.QuizName} | {quiz.Subject} | <{quiz.Grade}>";

            ViewData["AssociativeKey"] = quiz.QuizId;

            dynamic dynamicObj = new ExpandoObject();

            var responses = await _context.Responses.Where(m => m.AssociativeKey == AssociativeKey).ToListAsync();

            dynamicObj.ResposeList = responses;

            return View(dynamicObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuizQuestions(QuizResponse response)
        {
            if (ModelState.IsValid)
            {
                response.Id = Utility.GenerateGuid();

                response.IsActive = true;

                response.CreatedBy = Utility.loggedInUser;

                response.CreatedOn = Utility.OnGetCurrentDateTime();

                var questions = await _context.Responses.AddAsync(response);

                if (questions != null)
                {
                    int rc = await _context.SaveChangesAsync();

                    if (rc > 0)
                    {
                        _notify.Success("Question successfully saved", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save question!", 5);

                    }
                }
                else
                {
                    _notify.Error("Error: Soemthing went wrong!", 5);

                }
            }
            else
            {
                _notify.Error("Error: Enter a question!", 5);

            }

            return RedirectToAction(nameof(QuizQuestions), new { AssociativeKey = response.AssociativeKey });
        }

        [HttpGet]
        public async Task<IActionResult> OnQuizQuestions(Guid ResponseId)
        {
            if(ResponseId == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var getObj = await _context.Responses.Where(m => m.AssociativeKey == ResponseId).FirstOrDefaultAsync();

            return View(getObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnQuizQuestions(QuizResponse response)
        {
      
            var resp = await _context.Responses.Where(m => m.Id == response.AssociativeKey).FirstOrDefaultAsync();

            if(resp != null)
            {
                resp.Question = response.Question;

                resp.Answer = response.Answer;

                int rc = await _context.SaveChangesAsync();

                if(rc > 0)
                {
                    _notify.Success("Question successfully modified", 5);
                }
                else
                {
                    _notify.Success("Error: Unable to modify question", 5);
                }

            }
            else
            {
                _notify.Success("Error: Something went wrong", 5);

            }

            return RedirectToAction(nameof(QuizQuestions), new { AssociativeKey = response.AssociativeKey });

        }
        public async Task<IActionResult> OnBeginQuiz(QuizStart model, string RSAIDNumber)
        {
          
            if (String.IsNullOrEmpty(RSAIDNumber))
            {
              

                return RedirectToAction("NotFound", "Global");

            }

            Learner learner = await _context.Learners.FirstOrDefaultAsync(m => m.RSAIDNumber.Equals(RSAIDNumber));

            if (learner != null)
            {
                var quiz = await _context.Quiz.Where(x => x.Grade == learner.Grade).FirstOrDefaultAsync();

                ViewData["det"] = $"{learner.Name} {learner.LastName}";

                if (ModelState.IsValid)
                {
                    var qStart = await _context.QuizStart.AddAsync(model);

                    if (qStart != null)
                    {
                        int rc = await _context.SaveChangesAsync();

                        if (rc > 0)
                        {
                            _notify.Success("Quiz successfully", 5);
                            _notify.Warning("Please select the next question", 5);
                        }
                        else
                        {
                            _notify.Error("Error: Unable to complete operation!", 5);
                        }
                    }
                    else
                    {
                        _notify.Error("Error: Something went wrong!", 5);
                    }
                }
            }

            return RedirectToAction(nameof(OnBeginQuiz), new { Id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GradeQuiz()
        {
            var learners = await _context.Learners.ToListAsync();

            var quizzes = await _context.Quiz.ToListAsync();

            IEnumerable<SelectListItem> getLearner = from s in learners

                                                     select new SelectListItem
                                                     {
                                                         Value = s.LearnerId.ToString(),

                                                         Text = $"{s.Name} {s.LastName}"
                                                     };

            IEnumerable<SelectListItem> getQuizzes = from s in quizzes

                                                     select new SelectListItem
                                                     {
                                                         Value = s.QuizId.ToString(),

                                                         Text = $"{s.QuizName} <{s.Subject}>"
                                                     };

            ViewBag.LearnerId = new SelectList(getLearner, "Value", "Text");

            ViewBag.QuizId = new SelectList(getQuizzes, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeQuiz(GradeQuiz gradeQuiz)
        {

            if (ModelState.IsValid)
            {
                var grade = await _context.GradeQuiz.AddAsync(gradeQuiz);

                if (grade != null)
                {
                    int rc = await _context.SaveChangesAsync();

                    if (rc > 0)
                    {
                        _notify.Success("Learner Assessment Graded Successfully", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to grade learner quiz!", 5);
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            return View();
        }

        public async Task PopulateQuestions(QuizStart quiz)
        {
            var qResponse = await _context.Responses.Where(m => m.AssociativeKey == quiz.AssociativeId).ToListAsync();

            IEnumerable<SelectListItem> responseQ = from s in qResponse

                                                    select new SelectListItem
                                                    {
                                                        Value = s.AssociativeKey.ToString(),

                                                        Text = $"{s.Question}"
                                                    };

            ViewBag.AssociativeId = new SelectList(responseQ, "Value", "Text");

            ViewData["response"] = qResponse;
        }
    }
}
