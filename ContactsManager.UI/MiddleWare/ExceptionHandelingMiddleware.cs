using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace CRUDEXAMPLE.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandelingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandelingMiddleware> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public ExceptionHandelingMiddleware(RequestDelegate next , ILogger<ExceptionHandelingMiddleware> logger , IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);
                }
                else
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}", ex.GetType().ToString(), ex.Message);
                }
                //httpContext.Response.StatusCode = 500;
                //await httpContext.Response.WriteAsync("Error occured");
                throw;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHanelingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHanelingMiddleware(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<ExceptionHandelingMiddleware>();
        }
    }
}
