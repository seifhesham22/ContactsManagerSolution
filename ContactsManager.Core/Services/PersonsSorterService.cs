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
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsSorterService(IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("getSortedPersons PersonService");
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }
            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
            {
                (nameof(PersonResponse.Name), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Name), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Gender).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Gender).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Country).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Country).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Address).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Address).ToList(),

                (nameof(PersonResponse.RecieveNewsLetter), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.RecieveNewsLetter).ToList(),
                (nameof(PersonResponse.RecieveNewsLetter), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.RecieveNewsLetter).ToList(),

                _ => allPersons

            };
            return sortedPersons;

        }

    }
}
