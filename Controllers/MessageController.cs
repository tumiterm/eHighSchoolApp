using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class MessageController : Controller
    {
        private readonly IUnitOfWork<Message> _context;
        private readonly IUnitOfWork<Learner> _learnerContext;        public INotyfService _notify { get; }
        public MessageController(IUnitOfWork<Message> context,
                                 IUnitOfWork<Learner> learnerContext,
                                 INotyfService notify)
        {
            _context = context;

            _notify = notify;

            _learnerContext = learnerContext;

        }

        [HttpGet]
        public async Task<IActionResult> OnSendMessage()
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
        public async Task<IActionResult> OnSendMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                message.IsActive = true;

                message.MessageId = Utility.GenerateGuid();

                message.CreatedBy = Utility.loggedInUser;

                message.CreatedOn = Utility.OnGetCurrentDateTime();

                var messageObj = await _context.OnItemCreationAsync(message);

                if(messageObj != null)
                {
                    int rc = await _context.ItemSaveAsync();

                    if(rc > 0)
                    {
                        _notify.Success("Message successfully sent", 5);
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
                _notify.Error("Error: Enter all fields!", 5);
            }

            return View();
        }
    }
}
