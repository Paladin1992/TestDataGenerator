using Serilog;
using System;
using System.Web.Mvc;
using System.Web.Security;
using TestDataGenerator.Common;
using TestDataGenerator.Data.Models;
using TestDataGenerator.Resources;
using TestDataGenerator.Services;
using TestDataGenerator.Services.Models;
using TestDataGenerator.Web.Models;

namespace TestDataGenerator.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = model.Email.Trim();
                if (_accountService.GetUserByEmail(email) != null) // user exists
                {
                    ModelState.AddModelError("Email", Messages.Error_EmailAddress_Exists);
                    return View(model);
                }
                else // user does not exist -> register
                {
                    var (salt, hash) = PBKDF2.HashPassword(model.Password);

                    var user = new User()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = hash,
                        PasswordSalt = salt,
                        CreateDate = DateTime.Now
                    };

                    var userCreatedSuccess = _accountService.CreateAccount(user);

                    if (userCreatedSuccess)
                    {
                        var emailModel = new RegSuccessEmailModel()
                        {
                            To = model.Email,
                            FullName = model.Name,
                            Email = model.Email,
                            Password = model.Password
                        };

                        _emailService.CreateAndSend(emailModel, this);

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        Log.Error("User creation failed.");
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _accountService.GetUserByEmail(model.Email);                
                
                // if user exists and passwords match --> successful login
                if (user != null && PBKDF2.ValidatePassword(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    Session.Clear();
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["SessionEnd"] = DateTime.Now.AddMinutes(Session.Timeout);

                    return RedirectToAction("Index", "Home");
                }
                else // user does not exist or passwords don't match
                {
                    ModelState.AddModelError("", Messages.Error_Login_InvalidEmailOrPassword);
                }
            }

            return View(model);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult ForgottenPassword()
        {
            var model = new ForgottenPasswordViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassword(ForgottenPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    var user = _accountService.GetUserByEmail(model.Email);

                    if (user != null)
                    {
                        user.PasswordVerifyCode = Guid.NewGuid().ToString("N");
                        user.PasswordVerifyCodeExpirationDate = DateTime.Now.AddMinutes(AppConfig.Account.SecurityCodeValidMinutes);
                        _accountService.UpdateAccount(user);

                        var link = Url.Action("PasswordChange", "Account", new
                        {
                            code = user.PasswordVerifyCode,
                            email = user.Email
                        },
                        Request.Url.Scheme);

                        var emailModel = new ForgottenPasswordEmailModel()
                        {
                            FullName = user.Name,
                            To = model.Email,
                            VerifyLink = link,
                            ExpirationDate = user.PasswordVerifyCodeExpirationDate.ToString()
                        };

                        _emailService.CreateAndSend(emailModel, this);

                        TempData["EmailSent"] = string.Format(Messages.Info_ForgottenPw_VerifyEmailSent,
                                                              emailModel.ExpirationDate);
                    }
                    else // user does not exist --> redirect to register
                    {
                        return RedirectToAction("Register", "Account");
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult PasswordChange(string email, string code)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(code))
            {
                var tmpEmail = email ?? User.Identity.Name;
                var user = _accountService.GetUserByEmail(tmpEmail);

                if (user != null && user.PasswordVerifyCode == code)
                {
                    var model = new PasswordChangeViewModel()
                    {
                        Email = tmpEmail
                    };

                    return View(model);
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordChange(PasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _accountService.GetUserByEmail(model.Email);

                if (user != null)
                {
                    // if verification link hasn't expired yet
                    if (user.PasswordVerifyCodeExpirationDate.HasValue
                        && user.PasswordVerifyCodeExpirationDate.Value <= DateTime.Now)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else // new password -> change
                    {
                        try
                        {
                            _accountService.ChangePassword(user, model.Password);

                            TempData["PasswordChanged"] = Messages.Success_PasswordChanged;

                            FormsAuthentication.SignOut();
                            Session.Clear();

                            return RedirectToAction("Login", "Account");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", Messages.Error_ForgottenPassword);
                            Log.Error(ex, ex.Message);
                        }
                    }
                }
            }

            return View(model);
        }
    }
}