using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Dynamic;

namespace SchoolApp.Controllers
{
    [Authorize(Roles = "Admin,Parent")]

    public class ParentController : Controller
    {
        private readonly IUnitOfWork<Parent> _context;
        
        private readonly IUnitOfWork<Learner> _learnerContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public ParentController(IUnitOfWork<Parent> context, IUnitOfWork<Learner> learnerContext,
                                IWebHostEnvironment hostEnvironment,
                                INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _learnerContext = learnerContext;

            _hostEnvironment = hostEnvironment;

            _notify = notify;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.OnLoadItemsAsync());
        }

        public async Task<IActionResult> RegisterLearnerParent()
        {

            var learners = await _learnerContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> getLearner = from s in learners

                                                      select new SelectListItem
                                                      {
                                                          Value = s.LearnerId.ToString(),

                                                          Text = $"{s.Name} {s.LastName} | ({s.Grade})"
                                                      };


            ViewBag.LearnerId = new SelectList(getLearner, "Value", "Text");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterLearnerParent(Parent parent)
        {
            if (ModelState.IsValid)
            {
                parent.ParentId = Utility.GenerateGuid();

                parent.IsActive = true;

                parent.CreatedBy = Utility.loggedInUser;

                parent.CreatedOn = Utility.OnGetCurrentDateTime();

                if(!_context.DoesEntityExist<Parent>(m => m.RSAID == parent.RSAID))
                {
                    var obj = await _context.OnItemCreationAsync(parent);

                    if(obj != null)
                    {
                        int rc = await _context.ItemSaveAsync();

                        if(rc > 0)
                        {
                            _notify.Success("Parent successfully registered", 5);
                        }
                        else
                        {
                            _notify.Error("Error: Unable to register parent!",5);
                        }
                    }
                    else
                    {
                        _notify.Error("Error: Something went wrong!",5);

                    }
                }
                else
                {
                    _notify.Error("Error: Parent already exists!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Enter all fields", 5);

            }

            return RedirectToAction("OnRegisteredParent", "Parent", new { ParentId = parent.ParentId });
        }

        [HttpGet]
        public async Task<IActionResult> OnRegisteredParent(Guid ParentId)
        {
            if(ParentId == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var learner = await _context.OnLoadItemAsync(ParentId);

            if(learner is null)
            {
                return RedirectToAction("NotFound", "Global");
            }


            var learners = await _learnerContext.OnLoadItemsAsync();

            IEnumerable<SelectListItem> getLearner = from s in learners

                                                     select new SelectListItem
                                                     {
                                                         Value = s.LearnerId.ToString(),

                                                         Text = $"{s.Name} {s.LastName} | ({s.Grade})"
                                                     };


            ViewBag.LearnerId = new SelectList(getLearner, "Value", "Text");

            return View(learner);
        }
        public async Task<IActionResult> OnRegisteredParent(Parent parent)
        {
            if (ModelState.IsValid)
            {
                var parentObj = await _context.OnModifyItemAsync(parent);

                if(parent.LearnerId != Guid.Empty)
                {
                    if (parentObj != null)
                    {
                        _notify.Success("Learner Guardian successfully added", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to add learner parent",5);
                    }
                }
                else
                {
                    _notify.Warning("Warning: Parent still NOT linked to a learner", 5);
                }
            }

            return View();
        }
        public async Task<IActionResult> AddLearnerParent(Guid LearnerId)
        {
            if(LearnerId == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var learner = await _context.OnLoadItemAsync(LearnerId);

            if(learner is null)
            {
                return NotFound();
            }

            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLearnerParent(Parent parent)
        {
            if (ModelState.IsValid)
            {
                parent.ParentId = Utility.GenerateGuid();

                parent.CreatedOn = Utility.OnGetCurrentDateTime();

                parent.IsActive = true;

                var addParent = await _context.OnItemCreationAsync(parent);

                if(addParent != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success($"Parent: {parent.LastName} successfully registered!", 5);
                    }
                    else
                    {
                        _notify.Error($"Error: Unable to register parent!");
                    }
                }
                else
                {
                    _notify.Error("Error: Something went wrong!", 5);
                }
            }
            else
            {
                _notify.Error("Error: Something went wrong [Model]!", 5);
            }

            return RedirectToAction("SubjectAddition", "Subject", new {LearnerId = parent.LearnerId});
        }
        public async Task<IActionResult> OnAddLearnerParent(Guid ParentId)
        {
            if (ParentId == Guid.Empty)
            {
                return NotFound();
            }

            var parent = await _context.OnLoadItemAsync(ParentId);

            if (parent == null)
            {
                return NotFound();
            }

            var learnerParent = await _learnerContext.OnLoadItemsAsync();

            var onFilterParents = from n in learnerParent

                                  where n.LearnerId == parent.LearnerId

                                  select n;

            dynamic learnerParObj = new ExpandoObject();

            learnerParObj.ParentModel = onFilterParents.ToList();

            return View(learnerParent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnAddLearnerParent(Guid id,Parent parent)
        {
            if (id != parent.ParentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   var getParent = await _context.OnModifyItemAsync(parent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parent);
        }

        public async Task<IActionResult> OnRemoveTeacher(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.OnLoadItemAsync(id);

            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        [HttpPost, ActionName("OnRemoveTeacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var parent = await _context.OnLoadItemAsync(id);

            if (parent != null)
            {
                 _context.OnRemoveItemAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }  
    }
}
