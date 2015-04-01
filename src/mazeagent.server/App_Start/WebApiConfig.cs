using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using mazeagent.server.Infrastructure;

namespace mazeagent.server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new ApiExceptionHandler());

            config.MapHttpAttributeRoutes();

            // this route will catch everything that is not covered by a known route
            // and return a 404.
            config.Routes.MapHttpRoute(
                name: "everything-else",
                routeTemplate: "api/{*url}",
                defaults: new {controller = "ErrorApi", action = "Handle404"}
                );

            config.EnableCors();
        }
    }
}
