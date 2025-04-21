using CRUDEXAMPLE.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace CRUDEXAMPLE.Filters.ActionFilters
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonListActionFilter> _logger;
        public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(PersonListActionFilter), nameof(OnActionExecuting));
            PersonsController person = (PersonsController)context.Controller;
            IDictionary<string, object?>? Parameters = (IDictionary<string, object?>?)context.HttpContext.Items["Args"];
            if (Parameters != null)
            {
                if (Parameters.ContainsKey("searchBy"))
                {
                    person.ViewData["CurrentSearchBy"] = Convert.ToString(Parameters["searchBy"]);
                }
                if (Parameters.ContainsKey("searchString"))
                {
                    person.ViewData["CurrentSearchString"] = Convert.ToString(Parameters["searchString"]);
                }
                if (Parameters.ContainsKey("sortBy"))
                {
                    person.ViewData["CurrentSortBy"] = Convert.ToString(Parameters["sortBy"]);
                }
                else
                {
                    person.ViewData["CurrentSortBy"] = nameof(PersonResponse.Name);  
                }
                if (Parameters.ContainsKey("sortOrder"))
                {
                    person.ViewData["CurrentSortOrder"] = Convert.ToString(Parameters["sortOrder"]);
                }
                else
                {
                    person.ViewData["CurrentSortOrder"] = nameof(SortOrderOptions.ASC);
                }
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName.MethodName}" , nameof(PersonListActionFilter) , nameof(OnActionExecuting));

            context.HttpContext.Items["Args"] = context.ActionArguments;
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if (searchBy != null)
                {
                    List<string> search = new List<string>()
                            {
                                nameof(PersonResponse.Name),
                                nameof(PersonResponse.Email),
                                nameof(PersonResponse.Address),
                                nameof(PersonResponse.Country),
                                nameof(PersonResponse.DateOfBirth),
                                nameof(PersonResponse.Age),
                                nameof(PersonResponse.Gender),
                                nameof(PersonResponse.CountryId)
                            };
                    if (search.Any(x => x == searchBy) == false)
                    {
                        _logger.LogInformation("search by actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.Name);
                        _logger.LogInformation("search by updated value {searchBy}", searchBy);
                    }
                }
            }
        }
    }
}
    