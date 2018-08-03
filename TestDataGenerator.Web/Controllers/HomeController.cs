using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
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

        // DEBUG ONLY
        private void DebugDbSet<T>()
        {
            ViewData[typeof(T).Name + "s"] = JsonConvert.SerializeObject(_dataService.GetAll<T>().ToList());
        }

        public ActionResult Index()
        {
            DebugDbSet<User>();
            DebugDbSet<UserSetup>();

            return View();
        }

        public void RemoveAllUsers()
        {
            _dataService.RemoveAllRecords<User>();
        }
    }
}