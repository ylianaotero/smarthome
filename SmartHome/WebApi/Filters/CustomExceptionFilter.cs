using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private const string NotImplementedExceptionMessage = "Not implemented";
        private const string SomethingWentWrongMessage = "Something went wrong";
        
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = NotImplementedExceptionMessage })
                {
                    StatusCode = 500
                };
            }
            if (context.Exception is Exception)
            {
                context.Result = new ObjectResult(new { ErrorMessage = SomethingWentWrongMessage })
                {
                    StatusCode = 500
                };
            }
        }
    }
}