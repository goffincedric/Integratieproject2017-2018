using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    public class ErrorController : Controller
    {
    public ViewResult Index()
    {
      return View();
    }

    public ViewResult NotFound()
    {
      Response.StatusCode = 404;
      return View();
    }

    public ViewResult BadRequest()
    {
      Response.StatusCode = 500;
      return View();
    }
  }
}