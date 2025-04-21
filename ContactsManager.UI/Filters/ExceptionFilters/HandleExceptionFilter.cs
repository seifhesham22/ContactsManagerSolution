using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System.Net.Mime;

namespace CRUDEXAMPLE.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnviroment)
        {
            _logger = logger;
            _hostEnvironment = hostEnviroment;
        }
        public void OnException(ExceptionContext context)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                _logger.LogError("Exception Filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}",
                nameof(HandleExceptionFilter), nameof(OnException), context.Exception.GetType().ToString(), context.Exception.Message);
                context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = StatusCodes.Status500InternalServerError };

            }
        }
    }
}