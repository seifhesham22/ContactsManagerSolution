using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsDeleterService : IPersonsDeleterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsDeleterService(IPersonsRepository personsRepository, ILogger<PersonsDeleterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<bool> DeletePerson(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Person? person = await _personsRepository.GetPersonById(id.Value);

            if (person == null)
            {
                return false;
            }
            await _personsRepository.DeletePersonByPersonId(id.Value);
            return true;

        }
    }
}
