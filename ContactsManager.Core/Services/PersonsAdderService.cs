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
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsAdderService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));
            ValidationHelper.ModelValidation(personAddRequest);
            Person? person = personAddRequest.ToPerson();
            person.Id = Guid.NewGuid();
            await _personsRepository.AddPerson(person);

            //_db.sp_InsertPerson(person);
            return person.ToPersonResponse();
        }
    }
}
