using CRUDEXAMPLE.Filters;
using CRUDEXAMPLE.Filters.ActionFilters;
using CRUDEXAMPLE.Filters.AuthorizationFilters;
using CRUDEXAMPLE.Filters.ExceptionFilters;
using CRUDEXAMPLE.Filters.ResourceFilters;
using CRUDEXAMPLE.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDEXAMPLE.Controllers
{
    [Route("[controller]")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "Key-From-Controller", "Value-From-Controller" , 3} ,Order = 3)]
    [ResponseHeaderFilterFactory("Key-From-Controller", "Value-From-Controller", 3)]
    [TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;


        public PersonsController(IPersonsGetterService personsService,IPersonsDeleterService personsDeleter ,IPersonsAdderService personsAdder,IPersonsSorterService personsSorter ,IPersonsUpdaterService personsUpdater, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personsGetterService = personsService;
            _countriesService = countriesService;
            _logger = logger;
            _personsAdderService = personsAdder;
            _personsUpdaterService = personsUpdater;
            _personsSorterService = personsSorter;
            _personsDeleterService = personsDeleter;


        }


        [Route("[action]")]
        [Route("/")]
        [ServiceFilter(typeof(PersonListActionFilter) , Order = 4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter) , Arguments = new object[] { "Key-From-Action","Value-From-Action" , 1} , Order =1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        [ResponseHeaderFilterFactoryAttribute("Key-From-Action", "Value-From-Action" , 1)]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of persons controller");
            _logger.LogDebug($"searchby: {searchBy} searchString: {searchString} SortBy: {sortBy}");
            List<PersonResponse> response = await _personsGetterService.GetFilteredPersons(searchBy, searchString);
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.Name),"Person Name" },
                { nameof(PersonResponse.Email) , "Email"},
                { nameof(PersonResponse.DateOfBirth) , "Date of Birth"},
                { nameof(PersonResponse.Gender) , "Gender"},
                { nameof(PersonResponse.Country) , "Country"},
                { nameof(PersonResponse.Address) , "Address"},
            };
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchString = searchString;

            //sort
            List<PersonResponse> sortedPersons = await _personsSorterService.GetSortedPersons(response, sortBy, sortOrder);
            //ViewBag.CurrentSortBy = sortBy;
            //ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons);
        }



        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> response = await _countriesService.GetCountryList();
            ViewBag.CountryList = response.Select(x => new SelectListItem { Text = x.CountryName, Value = x.Id.ToString() });
            return View();
        }



        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisableResourceFilter),Arguments = new object[] { true})]
        public async Task<IActionResult> Create(PersonAddRequest person)
        {
          
            PersonResponse seif = await _personsAdderService.AddPerson(person);
            return RedirectToAction("Index", "Persons");
        }



        [HttpGet]
        [Route("[action]/{Id}")]
        [TypeFilter(typeof(TokenResultFilter))]
        [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]

        public async Task<IActionResult> Edit(Guid Id)
        {
            PersonResponse response = await _personsGetterService.GetPersonById(Id);
            if (response == null)
            {
                return RedirectToAction("Index");
            }
            PersonUpdateRequest personUpdateRequest = response.ToPersonUpdateRequest();

            List<CountryResponse> Response = await _countriesService.GetCountryList();
            ViewBag.CountryList = Response.Select(x => new SelectListItem { Text = x.CountryName, Value = x.Id.ToString() });
            return View(personUpdateRequest);
        }




        [HttpPost]
        [Route("[action]/{Id}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        //[TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest person)
        {
            PersonResponse personResponse = await _personsGetterService.GetPersonById(person.Id);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            await _personsUpdaterService.UpdatePerson(person);
            return RedirectToAction("Index");
        }



        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            PersonResponse personResponse = await _personsGetterService.GetPersonById(id);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }



        [HttpPost]
        [Route("[action]/{Id}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest request)
        {
            PersonResponse personResponse = await _personsGetterService.GetPersonById(request.Id);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            await _personsDeleterService.DeletePerson(request.Id);
            return RedirectToAction("Index");
        }



        [Route("[action]")]
        public async Task<IActionResult> PersonPDF()
        {
            List<PersonResponse> responses = await _personsGetterService.GetAllPersons();
            return new ViewAsPdf("PersonsPdf", responses, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }



        [Route("[action]")]
        public async Task<IActionResult> PersonCSV()
        {
            MemoryStream stream = await _personsGetterService.GetPersonsCSV();
            return File(stream ,"application/octet-stream" , "persons.csv");
        }



        [Route("[action]")]
        public async Task<IActionResult> PersonEXCEL()
        {
            MemoryStream stream = await _personsGetterService.GetPersonsExcel();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
