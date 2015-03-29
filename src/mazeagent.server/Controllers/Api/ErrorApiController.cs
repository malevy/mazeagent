using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mazeagent.server.Controllers.Api
{
    public class ErrorApiController : ApiController
    {
        public HttpResponseMessage Handle404(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("the requested resource does not exist")
            };

            throw new HttpResponseException(response);
        }
    }
}
