using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonGetterServiceWithFewExcelFields : IPersonsGetterService
    {
        private readonly PersonsGetterService _personsGetterService;
        
        public PersonGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personsGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            return await _personsGetterService.GetFilteredPersons(searchBy, searchString);
        }

        public async Task<PersonResponse> GetPersonById(Guid? id)
        {
            return await _personsGetterService.GetPersonById(id);
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personsGetterService.GetPersonsCSV();
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Gender";
                worksheet.Cells["C1"].Value = "Age";
                using (ExcelRange range = worksheet.Cells["A1:C1"])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> personResponses = await GetAllPersons();
                foreach (PersonResponse response in personResponses)
                {
                    worksheet.Cells[row, 1].Value = response.Name;
                    worksheet.Cells[row, 5].Value = response.Gender;
                    worksheet.Cells[row, 7].Value = response.Age;
                    row++;
                }
                worksheet.Cells[$"A1:C{row}"].AutoFitColumns();
                await package.SaveAsync();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
