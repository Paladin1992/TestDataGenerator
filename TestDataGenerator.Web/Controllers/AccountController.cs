using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDataGenerator.Web.Models;

namespace TestDataGenerator.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            return View();
        }


        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout(LoginViewModel model)
        {
            return View();
        }
    }
}