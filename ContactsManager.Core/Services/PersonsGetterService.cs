using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using Entities;
using ServiceContracts;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Services.Helpers;
using ServiceContracts.Enums;
using System.Security.Cryptography.X509Certificates;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Exceptions;
namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsGetterService(IPersonsRepository personsRepository , ILogger<PersonsGetterService> logger , IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("getAllPersons PersonService");
            var persons = await _personsRepository.GetAllPersons();
            return  persons.Select(x => x.ToPersonResponse()).ToList();
            //return _db.Persons.ToList().Select(x => ConvertPersonToPersonResponse(x)).ToList();
            //return _db.sp_GetAllPersons().Select(x => ConvertPersonToPersonResponse(x)).ToList();
        }

        public async Task<PersonResponse> GetPersonById(Guid? id)
        {
            if (id == null) return null;
            Person? person = await _personsRepository.GetPersonById(id.Value);
            if (person == null)
            {
                return null;
            }
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            List<Person> persons;
            _logger.LogInformation("getFilteredPersons PersonService");
            using (SerilogTimings.Operation.Time("Time for filtered person from db"))
            {
                persons = searchBy switch

                {
                    nameof(PersonResponse.Name) =>
                       await _personsRepository.GetFilteredPersons(x => x.Name.Contains(searchString)),


                    nameof(PersonResponse.Email) =>
                        await _personsRepository.GetFilteredPersons(x => x.Email.Contains(searchString)),

                    nameof(PersonResponse.DateOfBirth) =>
                      await _personsRepository.GetFilteredPersons(x => x.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),

                    nameof(PersonResponse.Gender) =>
                       await _personsRepository.GetFilteredPersons(x => x.Gender.Contains(searchString)),

                    nameof(PersonResponse.Country) =>
                      await _personsRepository.GetFilteredPersons(x => x.Country.CountryName.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                       await _personsRepository.GetFilteredPersons(x => x.Address.Contains(searchString)),


                    _ => await _personsRepository.GetAllPersons()
                };
            }
            _diagnosticContext.Set("Persons" , persons);
            return persons.Select(x=>x.ToPersonResponse()).ToList();
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            //CsvWriter csvWriter = new CsvWriter(writer , CultureInfo.InvariantCulture , leaveOpen:true);
            CsvWriter csvWriter = new CsvWriter(writer,csvConfiguration);
            //csvWriter.WriteHeader<PersonResponse>();
            csvWriter.WriteField(nameof(PersonResponse.Name));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.NextRecord();
            List<PersonResponse> responseList = await GetAllPersons();
            foreach(PersonResponse response in responseList)
            {
                csvWriter.WriteField(response.Name);
                csvWriter.WriteField(response.Email);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }
            //await csvWriter.WriteRecordsAsync(responseList);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Country";
                worksheet.Cells["D1"].Value = "Date Of Birth";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Address";
                worksheet.Cells["G1"].Value = "Age";
                using(ExcelRange range = worksheet.Cells["A1:G1"])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> personResponses = await GetAllPersons();
                foreach(PersonResponse response in personResponses)
                {
                    worksheet.Cells[row,1].Value = response.Name;
                    worksheet.Cells[row, 2].Value = response.Email;
                    worksheet.Cells[row, 3].Value = response.Country;
                    if (response.DateOfBirth.HasValue)
                    {
                        worksheet.Cells[row, 4].Value = response.DateOfBirth?.ToString("yyyy-MM-dd");
                    }
                    worksheet.Cells[row, 5].Value = response.Gender;
                    worksheet.Cells[row, 6].Value = response.Address;
                    worksheet.Cells[row, 7].Value = response.Age;
                    row++;
                }
                worksheet.Cells[$"A1:G{row}"].AutoFitColumns();
                await package.SaveAsync();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
