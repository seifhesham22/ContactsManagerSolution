using AutoFixture;
using Castle.Core.Logging;
using CRUDEXAMPLE.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsUpdaterService _personsUpdater;
        private readonly IPersonsSorterService _personsSorter;
        private readonly IPersonsDeleterService _personsDeleter;
        private readonly IPersonsAdderService _personsAdder;
        private readonly ICountriesService _countriesService;
        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<IPersonsGetterService> _personsGetterMock;
        private readonly Mock<IPersonsAdderService> _personsAdderMock;
        private readonly Mock<IPersonsUpdaterService> _personsUpdaterMock;
        private readonly Mock<IPersonsSorterService> _personsSorterMock;
        private readonly Mock<IPersonsDeleterService> _personsDeleterMock;
        private readonly IFixture _fixture;
        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _countriesServiceMock = new Mock<ICountriesService>();
            _personsGetterMock = new Mock<IPersonsGetterService>();
            _personsDeleterMock = new Mock<IPersonsDeleterService>();
            _personsAdderMock = new Mock<IPersonsAdderService>();
            _personsSorterMock = new Mock<IPersonsSorterService>();
            _personsUpdaterMock = new Mock<IPersonsUpdaterService>();
            _loggerMock = new Mock<ILogger<PersonsController>>();

            _countriesService = _countriesServiceMock.Object;
            _personGetterService = _personsGetterMock.Object;
            _personsDeleter = _personsDeleterMock.Object;
            _personsSorter = _personsSorterMock.Object;
            _personsUpdater = _personsUpdaterMock.Object;
            _personsAdder = _personsAdderMock.Object;
            _logger = _loggerMock.Object;
            
        }
        #region Index
        [Fact]
        public async Task index_ShouldReturnView()
        {
            //Arrange
            List<PersonResponse> responses = _fixture.Create<List<PersonResponse>>();
            PersonsController personsController = new PersonsController(_personGetterService,_personsDeleter , _personsAdder , _personsSorter , _personsUpdater, _countriesService,_logger);
            _personsGetterMock.Setup(x => x.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(responses);
            _personsSorterMock.Setup(x => x.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(responses);
    
            //Act
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(responses);
        }
        #endregion


        #region
        [Fact]
        public async Task WithModelErrors()
        {
            //Arrange
            PersonAddRequest request = _fixture.Create<PersonAddRequest>();
            PersonResponse response = _fixture.Create<PersonResponse>();
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesServiceMock.Setup(x => x.GetCountryList()).ReturnsAsync(countries);
            _personsAdderMock.Setup(x => x.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(response);

            PersonsController personsController = new PersonsController(_personGetterService, _personsDeleter, _personsAdder, _personsSorter, _personsUpdater, _countriesService, _logger); 

            //Act
            personsController.ModelState.AddModelError("name error", "Invalid name");
            IActionResult result = await personsController.Create(request);


            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(request);

        }

        [Fact]
        public async Task WithNoModelErrors()
        {
            PersonAddRequest request = _fixture.Create<PersonAddRequest>();
            PersonResponse response = _fixture.Create<PersonResponse>();

            
            _personsAdderMock.Setup(x => x.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(response);

            PersonsController personsController = new PersonsController(_personGetterService, _personsDeleter, _personsAdder, _personsSorter, _personsUpdater, _countriesService, _logger);

            //Act
            IActionResult result = await personsController.Create(request);


            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion


    }
}
