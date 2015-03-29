using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace mazeagent.server.Infrastructure
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorResult(context.Request);
        }
    }
}