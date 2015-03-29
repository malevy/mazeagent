using System;
using System.Net;
using System.Web.Mvc;

namespace mazeagent.server.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Handle404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult UnhandledError()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}