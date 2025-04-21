using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEXAMPLE.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;
        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before method", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));
            context.HttpContext.Response.Headers["last-modified"] = "seif";
            await next();
            _logger.LogInformation("{FilterName}.{MethodName} - after method", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));
            


        }
    }
}
