using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.eNums;
using SchoolApp.Helper;
using SchoolApp.Models;
using SchoolApp.Services;
using SchoolApp.ViewModels;

namespace SchoolApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWork<User> _context;

        private ApplicationDbContext _dbContext;

        private IWebHostEnvironment _hostEnvironment;
        public INotyfService _notify { get; }
        public AuthController(IUnitOfWork<User> context, ApplicationDbContext dbContext,
                               IWebHostEnvironment hostEnvironment,
                               INotyfService notify)
        {
            _context = context;

            _hostEnvironment = hostEnvironment;

            _dbContext = dbContext;

            _notify = notify;

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnGetUsers()
        {
              return View(await _context.OnLoadItemsAsync());
        }

        [HttpGet]
        public IActionResult OnRegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnRegisterUser(User user)
        {
            bool Status = false;

            string message = "";

            #region Generate Activation Code 

            string time = DateTime.Now.ToString("hh:mm tt");

            string date = DateTime.Now.ToString("dddd, dd MMMM yyyy") + " " + time;

            user.ActivationCode = Utility.GenerateGuid();

            user.LastLoginDate = date;

            user.ResetPasswordCode = "";

            user.Id = Utility.GenerateGuid();

            #endregion

            #region  Password Hashing 

            user.Password = Utility.ValueEncryption(user.Password);

            user.ConfirmPassword = Utility.ValueEncryption(user.ConfirmPassword);

            #endregion

            user.IsEmailVerified = false;

            user.IsActive = true;

            user.CreatedOn = Utility.OnGetCurrentDateTime();

            user.ResetPasswordCode = "";

            switch (user.Role)
            {
                case eRegisterAs.Learner:
                    user.RoleManager = eRole.Learner;
                    break;

                case eRegisterAs.Teacher:
                    user.RoleManager = eRole.Teacher;
                    break;

                case eRegisterAs.Parent:
                    user.RoleManager = eRole.Parent;
                    break;

                default:
                    user.RoleManager = eRole.Admin;
                    break;
            }

            if (ModelState.IsValid)
            {
                #region Save to Database

                try
                {
                    if (!_context.DoesEntityExist<User>(m => m.Username == user.Username))
                    {
                        var userAddition = _context.OnItemCreationAsync(user);

                        if (userAddition != null)
                        {
                            int rc = await _context.ItemSaveAsync();

                            if (rc > 0)
                            {
                                ViewData["successMessage"] = $"User Successfully Registered";

                            }
                        }

                        SendVerificationLinkEmail(user.Username, user.ActivationCode.ToString(),"","VerifyAccount");

                        message = " Registration successful. Account activation link " +

                            " has been sent to your email: " + user.Username + "\nKindly click on the link to activate your account.\n" +

                            " Regards";

                        Status = true;

                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
                #endregion
            }
            else
            {
                message = "Registration Successful";
            }

            ViewBag.Message = message;

            ViewBag.Status = Status;

            return View(user);
        }
        public  IActionResult OnSignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnSignIn(UserViewModel user, string? ReturnUrl = "")
        {
            string message = "";

            if (!String.IsNullOrEmpty(user.EmailID))
            {
                if (!String.IsNullOrEmpty(user.Password))
                {
                    if (ModelState.IsValid)
                    {

                        var userInfo = await _dbContext.Users.Where(m => m.Username == user.EmailID

                        && m.Password == Utility.ValueEncryption(user.Password)).

                        FirstOrDefaultAsync();

                        if (userInfo != null)
                        {
                            var claims = new List<Claim>()
                            {
                               new Claim(ClaimTypes.NameIdentifier,userInfo.Id.ToString()),
                               new Claim(ClaimTypes.Name, userInfo.Name),
                               new Claim(ClaimTypes.Surname, userInfo.LastName),
                               new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                               new Claim(ClaimTypes.Role, userInfo.RoleManager.ToString()),
                               new Claim("SchoolAppCookie","Code")
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var principal = new ClaimsPrincipal(identity);

                            var currentUserName = identity.FindFirst(ClaimTypes.Name);

                            var currentUserSurname = identity.FindFirst(ClaimTypes.Surname);

                            var currentUserId = identity.FindFirst(ClaimTypes.NameIdentifier);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                            {
                                IsPersistent = user.RememberMe

                            });

                            if (!userInfo.IsEmailVerified)
                            {
                                ViewBag.Message = "Please verify your email first";

                                _notify.Error("email not verified");

                                return View();

                            }


                            if (string.Compare(Utility.ValueEncryption(user.Password), userInfo.Password) == 0)
                            {
                                int timeout = user.RememberMe ? 525600 : 20;

                                if (Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {

                                    Utility.loggedInUser = $"{currentUserName.Value} {currentUserSurname.Value}";

                                    userInfo.LastLoginDate = DateTime.Now.ToString();

                                    if (userInfo.RoleManager == eRole.Learner)
                                    {
                                        return RedirectToAction("LearnerBoard", "Home");

                                    }
                                    else if(userInfo.RoleManager == eRole.Parent)
                                    {
                                        return RedirectToAction("ParentLandingPage", "Home");

                                    }
                                    else if(userInfo.RoleManager == eRole.Teacher)
                                    {
                                        return RedirectToAction("TeacherLandingPage", "Home");

                                    }
                                    else if(userInfo.RoleManager == eRole.Admin)
                                    {
                                        return RedirectToAction("Index", "Home");

                                    }
                                 

                                }

                            }
                            else
                            {
                                message = "Invalid credentials provided";

                                _notify.Error("Invalid credentials provided");
                            }
                        }
                        else
                        {
                            message = "Error: An error occured!";

                            _notify.Error("Error: An error occured!");

                        }
                    }
                }
                else
                {
                    message = "Error: Invalid or Empty Password!";

                    _notify.Error("Error: Invalid or Empty Password!");
                }
            }
            else
            {
                message = "Error: Invalid or Empty Email Provided!";

                _notify.Error("Error: Invalid or Empty Email Provided!");
            }

            ViewBag.Message = message;

            return View();
        }


        public async Task<IActionResult> OnModifyUser(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return RedirectToAction("NotFound", "Global");
            }

            var user = await _context.OnLoadItemAsync(Id);

            ViewData["password"] = user.Password;

            ViewData["ConfirmPassword"] = user.ConfirmPassword;

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnModifyUser(Guid Id, User user)
        {
            if (Id != user.Id)
            {
                return RedirectToAction("NotFound", "Global");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var rc = await _context.OnModifyItemAsync(user);

                    if(rc != null)
                    {
                        _notify.Success("User successfully saved", 5);
                    }
                    else
                    {
                        _notify.Error("Error: Unable to save user", 5);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _context.DoesEntityExist<User>(m => m.Id == user.Id))
                    {
                        return RedirectToAction("NotFound", "Global");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(OnGetUsers));
            }
            return View(user);
        }
        public async Task<IActionResult> OnRemoveUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var user = await _context.OnLoadItemAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Utility.loggedInUser = String.Empty;

            return RedirectToAction("OnSignIn");
        }

        [HttpPost, ActionName("OnRemoveUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.OnLoadItemAsync(id);

            if (user != null)
            {
                _context.OnRemoveItemAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string Username)
        {
            string message = "";

            bool status = false;

            if (string.IsNullOrEmpty(Username))
            {
                message = "Error: Email is a required field!";

                _notify.Error("Error: Email is required!", 5);

            }
            else
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(m => m.Username == Username);

                if (user is null)
                {
                    message = "Error: Account not found!";

                    _notify.Error("Error: Email is required!", 5);

                }
                else
                {
                    string resetCode = Guid.NewGuid().ToString();

                    SendVerificationLinkEmail(user.Username, resetCode,"", "PasswordReset");

                    user.ResetPasswordCode = resetCode;

                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    int rc = await _dbContext.SaveChangesAsync();

                    if(rc > 0)
                    {
                        message = "Reset password link successfully sent to your email.";

                        _notify.Success("Password link sent to email", 5);
                    }
                    else
                    {
                        message = "Error: Unable to send password link";

                        _notify.Success("Error: Unable to send password link", 5);
                    }


                }
            }

            ViewBag.Message = message;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> VerifyAccount(string Id)
        {
            bool Status = false;

            //_dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            var user = await _dbContext.Users.Where(a => a.ActivationCode == new Guid(Id)).FirstOrDefaultAsync();

            if (user != null)
            {
                if (!user.IsEmailVerified)
                {
                    user.IsEmailVerified = true;

                    int rc = await _dbContext.SaveChangesAsync();

                    if (rc > 0)
                    {
                        Status = true;

                        _notify.Success("User account is succcessfully verified", 5);
                    }

                    else
                    {
                        _notify.Error("Unable to Verify User", 5);

                    }
                }
                else
                {
                    _notify.Error("Error: Account already verified");
                }
            }
            else
            {
                _notify.Error("Model Error!", 5);
            }

            ViewBag.Status = Status;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel passwordReset)
        {
            var message = string.Empty;

            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.ResetPasswordCode == passwordReset.ResetCode);

                if (user != null)
                {
                    user.Password = Utility.ValueEncryption(passwordReset.NewPassword);

                    user.ResetPasswordCode = "";

                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    int rc = await _dbContext.SaveChangesAsync();

                    if(rc > 0)
                    {
                        message = "New password updated successfully";

                        _notify.Success("Password Successfully updated");
                    }
                    else
                    {
                        _notify.Error("Password could NOT be changed!");

                    }

                }

            }

            return View();
        }

        public async Task<IActionResult> PasswordReset(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return RedirectToAction("NotFound", "Global");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.ResetPasswordCode == Id);

            if (user != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();

                model.ResetCode = Id;

                return View(model);
            }
            else
            {
                return RedirectToAction("NotFound", "Global");
            }
        }

        [NonAction]
        public string OnGetAbsoluteUrl()
        {
            return string.Concat(

                Request.Scheme,

                "://",

                Request.Host.ToUriComponent(),

                Request.PathBase.ToUriComponent(),

                Request.Path.ToUriComponent(),

                Request.QueryString.ToUriComponent());
        }

        public ActionResult SendVerificationLinkEmail(string emailID, string activationCode, string messageBod, string emailFor = "VerifyAccount")
        {
            var verifyUrl = emailFor + "/" + activationCode;

            string url = OnGetAbsoluteUrl().Replace("OnRegisterUser", "");

            var link = $"{url}{verifyUrl}";

            var fromEmail = new MailAddress("apprentice@forek.co.za", "iCode E-Tutor");

            var toEmail = new MailAddress(emailID, "High School Applicant");

            var fromEmailPassword = "P@55w0rd2022";

            string subject = "";

            string body = "";

            if (emailFor == "VerifyAccount")
            {
                subject = "Registration Confirmation!";

                body = "<strong>iCode E-Tutor App Registration</strong><br/><br/><br/> Account successfully created." +
                   " Thanks for registering with us. Please click on the link below to verify your account.<br/><br/>" +
                   "<strong>Account created on:<strong/> " + DateTime.Now.ToString("dddd,dd MMMM yyyy HH:mm tt") +
                   " <br/><br/><a href='" + link + "'>" + link + "</a> <br/>Warm Regards";

                 messageBod = body;
            }

            else if (emailFor == "PasswordReset")
            {
                subject = "Reset Password";

                body = "Need to reset your password? No problem! Just click the button</b> " +
                    "below to and you'll be good to go. If not - kindly ignore this email.<br/><br/><a href=" + link.Replace("PasswordReset","") + ">Reset Password link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.forek.co.za",

                Port = 587,

                EnableSsl = true,

                DeliveryMethod = SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,

                Body = body,

                IsBodyHtml = true
            })

                smtp.Send(message);

            return View();
        }
    }
}
