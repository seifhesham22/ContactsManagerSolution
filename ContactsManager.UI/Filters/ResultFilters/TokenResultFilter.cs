using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CRUDEXAMPLE.Filters.ResultFilters
{
    public class TokenResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
       
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append("Auth-Key","A100");
        }
    }
}
