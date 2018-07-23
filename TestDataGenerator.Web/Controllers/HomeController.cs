using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDataGenerator.Common;
using TestDataGenerator.Data.Models;
using TestDataGenerator.Services;

namespace TestDataGenerator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public ActionResult Index()
        {
            _dataService.AddUser(new User()
            {
                Name = "Marosvölgyi Gergely",
                PasswordHash = "1234".HashPassword(out string salt),
                PasswordSalt = salt,
                Email = "g.marosvolgyi@mortoff.hu"
            });

            _dataService.AddUser(new User()
            {
                Name = "Gönczi Krisztián",
                PasswordHash = "1234".HashPassword(out salt),
                PasswordSalt = salt,
                Email = "k.gonczi@mortoff.hu"
            });

            ViewBag.Users = JsonConvert.SerializeObject(_dataService.GetUsers().ToList());

            return View();
        }
    }
}