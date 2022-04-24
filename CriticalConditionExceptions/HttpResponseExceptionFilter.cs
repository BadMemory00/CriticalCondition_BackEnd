using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CriticalConditionBackend.CriticalConditionExceptions
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is LogicalException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = exception.statusCode
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
