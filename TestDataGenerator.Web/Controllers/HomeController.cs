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
    public class HomeController : BaseController
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public ActionResult Index()
        {
            ViewBag.Users = JsonConvert.SerializeObject(_dataService.GetUsers().ToList());

            return View();
        }
    }
}