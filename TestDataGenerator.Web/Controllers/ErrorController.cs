using System.Web;
using System.Web.Mvc;
using TestDataGenerator.Common;

namespace TestDataGenerator.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            if (new HttpRequestWrapper(System.Web.HttpContext.Current.Request).IsAjaxRequest())
            {
                return Json(new ResponseModel(), JsonRequestBehavior.AllowGet);
            }

            return View();
        }
    }
}