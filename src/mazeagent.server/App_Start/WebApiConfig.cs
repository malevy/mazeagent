using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using mazeagent.server.Infrastructure;
using mazeagent.server.Infrastructure.MediaFormatters;

namespace mazeagent.server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new ApiExceptionHandler());
            config.Formatters.Insert(0, new MazeXmlMediaFormatter()); // set as the first formatter
            config.Formatters.Insert(1, new CollectionJsonMediaFormatter());

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
