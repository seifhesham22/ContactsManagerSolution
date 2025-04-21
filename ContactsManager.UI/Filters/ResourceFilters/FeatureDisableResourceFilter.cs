using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.RateLimiting;

namespace CRUDEXAMPLE.Filters.ResourceFilters
{
    public class FeatureDisableResourceFilter :IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisableResourceFilter> _logger;
        private readonly bool _disable;
        public FeatureDisableResourceFilter(ILogger<FeatureDisableResourceFilter> logger, bool disable = true)
        {
            _logger = logger;
            _disable = disable;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            //before
            _logger.LogInformation("{FilterName}.{MethodName} before", nameof(FeatureDisableResourceFilter) , nameof(OnResourceExecutionAsync));
            if (_disable)
            {
                //context.Result = new NotFoundResult();
                context.Result = new StatusCodeResult(501); // not implemented;
            }
            else
            {
                await next();
            }
            //After
            _logger.LogInformation("{FilterName}.{MethodName} after", nameof(FeatureDisableResourceFilter), nameof(OnResourceExecutionAsync));

        }
    }
}
