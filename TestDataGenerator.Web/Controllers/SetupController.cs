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

        // DEBUG ONLY
        private string TestData(int count, List<FieldModel> fields)
        {
            for (int i = 0; i < count; i++)
            {
                for (int fieldIndex = 0; fieldIndex < fields.Count; fieldIndex++)
                {
                    var field = fields[fieldIndex];

                    if (field is FirstNameFieldModel)
                    {
                        _dataGeneratorService.GenerateFirstName(field as FirstNameFieldModel);
                    }
                    else if (field is LastNameFieldModel)
                    {
                        _dataGeneratorService.GenerateLastName(field as LastNameFieldModel);
                    }
                    else if (field is DateTimeFieldModel)
                    {
                        _dataGeneratorService.GenerateDateTime(field as DateTimeFieldModel);
                    }
                    else if (field is EmailFieldModel)
                    {
                        _dataGeneratorService.GenerateEmail(field as EmailFieldModel);
                    }
                    else if (field is TextFieldModel)
                    {
                        _dataGeneratorService.GenerateText(field as TextFieldModel);
                    }
                    else if (field is Int32FieldModel)
                    {
                        _dataGeneratorService.GenerateSignedInteger(field as Int32FieldModel);
                    }
                    else if (field is UInt32FieldModel)
                    {
                        _dataGeneratorService.GenerateUnsignedInteger(field as UInt32FieldModel);
                    }
                    else if (field is Int64FieldModel)
                    {
                        _dataGeneratorService.GenerateSignedLongInteger(field as Int64FieldModel);
                    }
                    else if (field is UInt64FieldModel)
                    {
                        _dataGeneratorService.GenerateUnsignedLongInteger(field as UInt64FieldModel);
                    }
                    else if (field is FloatFieldModel)
                    {
                        _dataGeneratorService.GenerateFloat(field as FloatFieldModel);
                    }
                    else if (field is DoubleFieldModel)
                    {
                        _dataGeneratorService.GenerateDouble(field as DoubleFieldModel);
                    }
                    else if (field is HashFieldModel)
                    {
                        _dataGeneratorService.GenerateUnsignedLongInteger(field as UInt64FieldModel);
                    }
                    else if (field is GuidFieldModel)
                    {
                        _dataGeneratorService.GenerateGuid(field as GuidFieldModel);
                    }
                    else if (field is CustomSetFieldModel)
                    {
                        _dataGeneratorService.GenerateFromCustomSet(field as CustomSetFieldModel);
                    }
                }

                _dataGeneratorService.GenerateDateTime(new DateTimeFieldModel("", new DateTime(1950, 1, 1), new DateTime(2050, 12, 31)));
            }

            return null;
        }

        public class TypeSwitch
        {
            Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();

            public TypeSwitch Case<T>(Action<T> action)
            {
                matches.Add(typeof(T), (x) => action((T)x));
                return this;
            }

            public void Switch(object x)
            {
                matches[x.GetType()](x);
            }
        }

#warning Implementálni kell
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
    }
}