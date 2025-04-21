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
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsUpdaterService(IPersonsRepository personsRepository, ILogger<PersonsUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }
            Helpers.ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = await _personsRepository.GetPersonById(personUpdateRequest.Id);
            if (matchingPerson == null)
            {
                throw new InvalidPersonIdException(nameof(personUpdateRequest));
            }

            //Updating
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.Name = personUpdateRequest.Name;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.RecieveNewsLetter = personUpdateRequest.RecieveNewsLetter;
            await _personsRepository.UpdatePerson(matchingPerson);
            return matchingPerson.ToPersonResponse();
        }
    }
}
