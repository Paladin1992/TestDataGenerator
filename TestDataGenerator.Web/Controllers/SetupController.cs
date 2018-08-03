using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDataGenerator.Services;
using TestDataGenerator.Web.Models;

namespace TestDataGenerator.Web.Controllers
{
    public class SetupController : Controller
    {
        private readonly IDataService _dataService;

        public SetupController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new SetupCreateViewModel();

            

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(SetupCreateViewModel model)
        {
            // TODO Create
            return View(model);
        }

#warning Implementálni kell
        [HttpGet]
        public ActionResult Edit()
        {
            var model = new SetupEditViewModel();

            return View();
        }

        [HttpPost]
        public ActionResult Edit(SetupEditViewModel model)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }
    }
}