using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDEXAMPLE.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [Route("[action]")]
        public IActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.Err = "Please select and excelFile";
                return View();
            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx" , StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Err = "Unsupported file , Upload Excel!";
            }
            int count =  await _countriesService.UploadCountriesFromExcelFile(excelFile);
            ViewBag.Count = $"{count} countries uploaded";
            return View();
        }
    }
}
