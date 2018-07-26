using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestDataGenerator.Common;
using TestDataGenerator.Data.Models;
using TestDataGenerator.Resources;
using TestDataGenerator.Services;
using TestDataGenerator.Web.Models;

namespace TestDataGenerator.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                    var user = new User()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = model.Password.HashPassword(out string salt),
                        PasswordSalt = salt,
                        CreateDate = DateTime.Now
                    };

                    var userCreatedSuccess = _accountService.CreateAccount(user);

                    if (userCreatedSuccess)
                    {
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
                return RedirectToAction("Index", "Account");
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
                var hashedPassword = model.Password.HashPassword(out string salt);
                
                // if user exists and passwords match --> successful login
                if (user != null && user.PasswordHash.Equals(hashedPassword, StringComparison.InvariantCultureIgnoreCase))
                {
                    Session.Clear();
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["SessionEnd"] = DateTime.Now.AddMinutes(Session.Timeout);

                    return RedirectToAction("Index", "Account");
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
    }
}