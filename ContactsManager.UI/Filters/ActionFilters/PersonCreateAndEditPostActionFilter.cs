using CRUDEXAMPLE.Controllers;
using Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;

namespace CRUDEXAMPLE.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;
        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> response = await _countriesService.GetCountryList();
                    personsController.ViewBag.CountryList = response
                    .Select(country => new SelectListItem
                    {
                        Text = country.CountryName,  // Displayed in the dropdown
                        Value = country.Id.ToString() // Actual value when selected
                    }).ToList();

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["person"];
                    context.Result =  personsController.View(personRequest); // short circuiting or skipping the subsequent action filters & action methods 
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
            
            
        }
    }
}
