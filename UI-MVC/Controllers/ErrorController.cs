using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    public class ErrorController : Controller
    {
    public ViewResult Index()
    {
      return View("Error");
    }

    public ViewResult NotFound()
    {
      Response.StatusCode = 404;
      return View("NotFound");
    }

    public ViewResult BadRequest()
    {
      Response.StatusCode = 500;
      return View("NotFound");
    }
  }
}