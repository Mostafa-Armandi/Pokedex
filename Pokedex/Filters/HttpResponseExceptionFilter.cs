using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Pokedex.Exceptions;

namespace Pokedex.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        private readonly ILogger<HttpResponseExceptionFilter> _logger;

        public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is RemoteApiException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = (int)exception.StatusCode
                };
                context.ExceptionHandled = true;
                _logger.LogError(exception, "Remote endpoint invocation finished with error");
            }
            else
            {
                _logger.LogError(context.Exception, "An unhandled exception occured");
            }
        }
    }
}