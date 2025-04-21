using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEXAMPLE.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private string _key;
        private string _value;
        private int _order;
        public ResponseHeaderFilterFactoryAttribute(string key , string value , int order)
        {
            _key = key;
            _value = value;
            _order = order;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = _key;
            filter.Value = _value;
            filter.Order = _order;
            return filter;
        }
    }
    public class ResponseHeaderActionFilter : IAsyncActionFilter , IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public string Key { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }

        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} before - method" , nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            await next();
            _logger.LogInformation("{FilterName}.{MethodName} after - method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
