using Azure;
using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsContrtollerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
         public PersonsContrtollerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        #region
        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrange
            
            //Act
            HttpResponseMessage message = await _client.GetAsync("Persons/Index");
            string read = await message.Content.ReadAsStringAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(read);
            var doc = document.DocumentNode;
            doc.QuerySelectorAll("table.persons").Should().NotBeNull();

            
            //Assert
            Assert.InRange((int)message.StatusCode, 200, 299);
        }
        #endregion
    }
}
