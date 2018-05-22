using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    /// <summary>
    ///     Has control over the error pages. Will come to action when error has been encountered.
    /// </summary

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