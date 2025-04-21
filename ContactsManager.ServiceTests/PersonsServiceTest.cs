using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using Xunit;
using Entities;
using System.ComponentModel.Design;
using Xunit.Abstractions;
using RepositoryContracts;
using Moq;
using FluentAssertions;
using AutoFixture;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Serilog;
using Serilog.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsGetterService _personsGetter;
        private readonly IPersonsUpdaterService _personsUpdater;
        private readonly IPersonsSorterService _personsSorter;
        private readonly IPersonsDeleterService _personsDeleter;
        private readonly IPersonsAdderService _personsAdder;
        private readonly ITestOutputHelper _outputHelper;
        private readonly IFixture _fixture;
        private readonly Mock<IPersonsRepository> _personMock;
        private readonly IPersonsRepository _personsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesRepository> _CountriesMock;
        public PersonsServiceTest(ITestOutputHelper helper)
        {

            _personMock = new Mock<IPersonsRepository>();
            _personsRepository = _personMock.Object;
            _CountriesMock = new Mock<ICountriesRepository>();
            _countriesRepository = _CountriesMock.Object;
            _fixture = new Fixture();
            var diagnosticContext = new Mock<IDiagnosticContext>();
            var logger = new Mock<ILogger<PersonsGetterService>>();
            var logger2 = new Mock<ILogger<PersonsUpdaterService>>();
            var logger3 = new Mock<ILogger<PersonsAdderService>>();
            var logger4 = new Mock<ILogger<PersonsDeleterService>>();
            var logger5 = new Mock<ILogger<PersonsSorterService>>();

            _personsGetter = new PersonsGetterService(_personsRepository ,logger.Object ,diagnosticContext.Object);
            _personsUpdater = new PersonsUpdaterService(_personsRepository,logger2.Object, diagnosticContext.Object);
            _personsAdder = new PersonsAdderService(_personsRepository, logger3.Object, diagnosticContext.Object);
            _personsDeleter = new PersonsDeleterService(_personsRepository, logger4.Object, diagnosticContext.Object);
            _personsSorter = new PersonsSorterService(_personsRepository, logger5.Object, diagnosticContext.Object);
            _outputHelper = helper;
            
        }
        #region Add Person
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _personsAdder.AddPerson(personAddRequest);
            });

        }
        [Fact]
        public async Task AddPerson_NullPersonName_ToThrowArgumentNullException()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { Name = null};
            Person person = personAddRequest.ToPerson();
            _personMock.Setup(x=>x.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _personsAdder.AddPerson(personAddRequest);
            });

        }
        [Fact]
        public async Task AddPerson_FullPersonDetails_BeSuccessful()
        {
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                Name = "seif",
                Address = "dawning street",
                DateOfBirth = DateTime.Today,
                Email = "seifhesham882@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = Guid.NewGuid(),
                RecieveNewsLetter = true
            };
            Person person = personAddRequest.ToPerson();
            PersonResponse expected_response = person.ToPersonResponse();
            _personMock.Setup(x => x.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            PersonResponse person_responce_from_add = await _personsAdder.AddPerson(personAddRequest);

            expected_response.Id = person_responce_from_add.Id; 
            person_responce_from_add.Id.Should().NotBe(Guid.Empty); 
            person_responce_from_add.Should().Be(expected_response);    
            

        }
        #endregion

        #region Get Person
        [Fact]
        public async Task GetPersonById_NullPersonId_ToBeNull()
        {
            Guid? id = null;
            PersonResponse? response_from_get = await _personsGetter.GetPersonById(id);
            Assert.Null(response_from_get);
        }
        [Fact]
        public async Task GetPersonById_WithPersonId_ToBeSuccessful()
        {
            //Arrange
            
            Person person = _fixture.Build<Person>().With(x=>x.Email , "seif@gmail.com").With(x=>x.Country , null as Country).Create();
            PersonResponse expected = person.ToPersonResponse();
            //Act
            _personMock.Setup(x=>x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            
            PersonResponse actual = await _personsGetter.GetPersonById(person.Id);
            //Assert
            actual.Should().Be(expected);
        }
        #endregion

        #region GetAllPersons
        //The aetallpersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EmptyList_ToBeEmpty()
        {
            var persons = new List<Person>(); 
            _personMock.Setup(x=>x.GetAllPersons()).ReturnsAsync(persons);
            List<PersonResponse> responce_from_get = await _personsGetter.GetAllPersons();
            responce_from_get.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
           List<Person> persons = new List<Person>()
           {
               _fixture.Build<Person>().With(x=>x.Email , "seif1@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif2@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif3@gmail.com").With(x=>x.Country, null as Country).Create(),
           };
           
            
            List<PersonResponse> expected = persons.Select(x => x.ToPersonResponse()).ToList();
            
            //print expected
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse expectedPersons in expected)
            {
                _outputHelper.WriteLine(expectedPersons.ToString());
            }


            //Act
            _personMock.Setup(x=>x.GetAllPersons()).ReturnsAsync(persons);
            List<PersonResponse> actual = await _personsGetter.GetAllPersons();


            //print person_response_From_GetAll
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse persons_list_from_Get in actual)
            {
                _outputHelper.WriteLine(persons_list_from_Get.ToString());
            }


            //Assert
            actual.Should().BeEquivalentTo(expected);



        }
        #endregion

        #region Get Filtered Persons
        //if search text is empty and searchBy = person name, return all persons.
        [Fact]
        public async Task GetAllPersons_EmptySearchText()
        {
            //Arrange
            List<Person> persons = new List<Person>()
           {
               _fixture.Build<Person>().With(x=>x.Email , "seif1@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif2@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif3@gmail.com").With(x=>x.Country, null as Country).Create(),
           };

            List<PersonResponse> expected = persons.Select(x => x.ToPersonResponse()).ToList();

            //print person_response_From_add_request
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_From_add in expected)
            {
                _outputHelper.WriteLine(person_response_From_add.ToString());
            }


            //Act
            _personMock.Setup(x=>x.GetFilteredPersons(It.IsAny<Expression<Func<Person,bool>>>())).ReturnsAsync(persons);
            List<PersonResponse> actual = await _personsGetter.GetFilteredPersons(nameof(Person.Name),"");


            //print person_response_From_GetAll
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse persons_list_from_Get in actual)
            {
                _outputHelper.WriteLine(persons_list_from_Get.ToString());
            }


            //Assert
            actual.Should().BeEquivalentTo(expected);

        }



        [Fact]
        //First we 
        public async Task GetAll_SearchByPersonName_ShouldBeSuccefull()
        {
            //Arrange
            List<Person> persons = new List<Person>()
           {
               _fixture.Build<Person>().With(x=>x.Email , "seif1@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif2@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif3@gmail.com").With(x=>x.Country, null as Country).Create(),
           };

            List<PersonResponse> expected = persons.Select(x => x.ToPersonResponse()).ToList();

            //print person_response_From_add_request
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_From_add in expected)
            {
                _outputHelper.WriteLine(person_response_From_add.ToString());
            }


            //Act
            _personMock.Setup(x => x.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            List<PersonResponse> actual = await _personsGetter.GetFilteredPersons(nameof(Person.Name), "sa");


            //print person_response_From_GetAll
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse persons_list_from_Get in actual)
            {
                _outputHelper.WriteLine(persons_list_from_Get.ToString());
            }


            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion


        #region Get Sorted Persons
        //when we sort based on the Person Name in DESC, it should return the person in DESC order it self
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
           {
               _fixture.Build<Person>().With(x=>x.Email , "seif1@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif2@gmail.com").With(x=>x.Country, null as Country).Create(),
               _fixture.Build<Person>().With(x=>x.Email , "seif3@gmail.com").With(x=>x.Country, null as Country).Create(),
           };
            List<PersonResponse> expected = persons.Select(x => x.ToPersonResponse()).ToList();

            

            //print person_response_From_add_request
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_From_add in expected)
            {
                _outputHelper.WriteLine(person_response_From_add.ToString());
            }


            //Act 
            _personMock.Setup(x=>x.GetAllPersons()).ReturnsAsync(persons);
            List<PersonResponse> allPersons = await _personsGetter.GetAllPersons();
            List<PersonResponse> persons_list_from_sort = await _personsSorter.GetSortedPersons(allPersons, nameof(Person.Name), SortOrderOptions.DESC);


            //print person_response_From_GetAll
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse persons_list_from_Get in persons_list_from_sort)
            {
                _outputHelper.WriteLine(persons_list_from_Get.ToString());
            }
           
            persons_list_from_sort.Should().BeInDescendingOrder(x=>x.Name);



        }
        #endregion

        #region Update Person
        //when we supply person update request as nul we have to throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            PersonUpdateRequest? Person = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _personsUpdater.UpdatePerson(Person);
            });

        }
        [Fact]
        public async Task UpdatePerson_InvalidPersonId_ToBeArgumentException()
        {
            PersonUpdateRequest? Person = new PersonUpdateRequest() { Id = Guid.NewGuid()};

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _personsUpdater.UpdatePerson(Person);
            });
        }
        [Fact]
        public async Task UpdatePerson_NUllPersonName_ToBeArgumentException()
        {
            //Arrange
            Person person = _fixture.Build<Person>().With(x=>x.Email , "seif@gmail.com").With(x=>x.Name, null as string).With(x=>x.Country , null as Country).With(x=>x.Gender , GenderOptions.Male.ToString()).Create();
            PersonResponse personResponse = person.ToPersonResponse();
            PersonUpdateRequest person_update_request = personResponse.ToPersonUpdateRequest();

            //Act
            var action = async () =>
            {
                await _personsUpdater.UpdatePerson(person_update_request);
            };


            await action.Should().ThrowAsync<ArgumentException>();
        }

        //update the person name and email 
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdate_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>().With(x => x.Email, "seif@gmail.com").With(x => x.Country, null as Country).With(x => x.Gender, GenderOptions.Male.ToString()).Create();

            PersonResponse Expected = person.ToPersonResponse();
            PersonUpdateRequest person_update_request = Expected.ToPersonUpdateRequest();
            _personMock.Setup(x => x.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
            _personMock.Setup(x => x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);

            //act
  
            PersonResponse responce_from_update = await _personsUpdater.UpdatePerson(person_update_request);
           
            //Assert
            responce_from_update.Should().Be(Expected);
        }
        #endregion

        #region Delete Person
        // valid person id? then true
        [Fact]
        public async Task DeletePerson_ValidId_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>().With(x => x.Email, "seif@gmail.com").With(x => x.Country, null as Country).With(x => x.Gender, GenderOptions.Male.ToString()).Create();
            _personMock.Setup(x => x.DeletePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(true);
            _personMock.Setup(x => x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            bool isDeleted = await _personsDeleter.DeletePerson(person.Id);
            //Assert
            isDeleted.Should().BeTrue();
        }

        [Fact]
        // invalid person id? then false
        public async Task DeletePerson_InvalidId_ToBeFalse()
        {
            Person person = _fixture.Build<Person>().With(x => x.Email, "seif@gmail.com").With(x => x.Country, null as Country).With(x => x.Gender, GenderOptions.Male.ToString()).Create();
            _personMock.Setup(x => x.DeletePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(false);
            _personMock.Setup(x => x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(null as Person);

            //Act
            bool isDeleted = await _personsDeleter.DeletePerson(person.Id);
            //Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }

}
