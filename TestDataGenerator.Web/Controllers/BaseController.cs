using Serilog;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestDataGenerator.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            Log.Information($"{filterContext.HttpContext.Request.RequestType} " +
                $"{controller}/{action} " +
                $"UserName: {filterContext.HttpContext.User?.Identity?.Name}");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"];
            var action = filterContext.RouteData.Values["action"];

            Log.Error($"{filterContext.HttpContext.Request.RequestType} " +
                $"{controller}/{action} " +
                $"UserName: {filterContext.HttpContext.User?.Identity?.Name}");

            filterContext.ExceptionHandled = true;

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Error", action = "Index" })
            );
        }
    }
}