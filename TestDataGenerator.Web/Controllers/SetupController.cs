using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using TestDataGenerator.Common;
using TestDataGenerator.Data;
using TestDataGenerator.Data.Enums;
using TestDataGenerator.Data.Models;
using TestDataGenerator.Services;
using TestDataGenerator.Services.Models;
using TestDataGenerator.Web.Models;

namespace TestDataGenerator.Web.Controllers
{
    public class SetupController : Controller
    {
        private readonly IDataService _dataService;

        private readonly DataGeneratorService _dataGeneratorService;

        private readonly ISetupService _setupService;

        private readonly IMapper _mapper;

        public SetupController(
            IDataService dataService,
            DataGeneratorService dataGeneratorService,
            ISetupService setupService,
            IMapper mapper)
        {
            _dataService = dataService;
            _dataGeneratorService = dataGeneratorService;
            _setupService = setupService;
            _mapper = mapper;

            // DEBUG ONLY
            if (!_setupService.GetAll().Any())
            {
                _setupService.AddOrUpdateSetup(new UserSetup()
                {
                    Id = 1,
                    Name = "Signal Partner",
                    CreateDate = new DateTime(2018, 8, 1),
                    Fields = new List<FieldModel>()
                });

                _setupService.AddOrUpdateSetup(new UserSetup()
                {
                    Id = 2,
                    Name = "Signal Commission",
                    CreateDate = new DateTime(2016, 5, 31),
                    Fields = new List<FieldModel>()
                });

                _setupService.AddOrUpdateSetup(new UserSetup()
                {
                    Id = 3,
                    Name = "Microsite",
                    CreateDate = new DateTime(2017, 9, 15),
                    Fields = new List<FieldModel>()
                });
            }
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList()
        {
            var userSetups = _mapper.Map<IEnumerable<ListItemUserSetupViewModel>>(_setupService.GetAll()).ToList();

            return Json(userSetups, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Update(ListItemUserSetupViewModel model)
        {
            _setupService.UpdateSetup(_mapper.Map<UserSetup>(model));
        }

#warning Implementálni kell
        [HttpGet]
        public ActionResult Create()
        {
            var model = new SetupCreateViewModel();

            ViewBag.FieldTypeInfos = GetFieldTypeInfos();

            return View(model);
        }

        
#warning Implementálni kell
        [HttpPost]
        public ActionResult Create(SetupCreateViewModel model)
        {
            // TODO
            return View(model);
        }

#warning Implementálni kell
        [HttpGet]
        public ActionResult Edit()
        {
            var model = new SetupEditViewModel();

            return View();
        }

#warning Implementálni kell
        [HttpPost]
        public ActionResult Edit(SetupEditViewModel model)
        {
            return View();
        }

#warning Implementálni kell
        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }


        #region [ Helpers ]
        private List<FieldTypeInfo> GetFieldTypeInfos()
        {
            var result = ((IEnumerable<FieldType>)Enum.GetValues(typeof(FieldType)))
                .Select(e => new FieldTypeInfo()
                {
                    Value = (int)e,
                    Name = e.ToString(),
                    Description = e.GetAttributeOfType<DescriptionAttribute>()?.Description ?? "",
                    HasMinValue = e.GetAttributeOfType<HasExtremeAttribute>()?.HasMinValue ?? false,
                    HasMaxValue = e.GetAttributeOfType<HasExtremeAttribute>()?.HasMaxValue ?? false
                }).ToList();

            return result;
        }

        #endregion
    }
}