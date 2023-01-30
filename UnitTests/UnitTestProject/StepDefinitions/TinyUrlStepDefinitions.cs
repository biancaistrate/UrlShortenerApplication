using Azure;
using Domain.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WebApi.Models;

namespace SpecFlow.Api.Tests.Acceptance.StepDefinitions
{
    [Binding]
    public class TinyUrlStepDefinitions
    {
        private const string BaseAddress = "http://localhost/";
        public WebApplicationFactory<Program> Factory { get; }
        public HttpClient Client { get; set; } = null!;

        public TinyUrlStepDefinitions(WebApplicationFactory<Program> factory)
        {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
            Client.BaseAddress = new Uri(BaseAddress);
        }

        [When(@"I create tinyUrls with the following details")]
        public async Task WhenICreateTinyUrlsWithTheFollowingDetails(Table table)
        {
            await RegisterAndLogin();

            var tinyUrls = table.CreateSet<TinyUrlDTO>();
            foreach (var tinyUrl in tinyUrls)
            {
                var response = await Client.PutAsJsonAsync("/tinyurl", tinyUrl);
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }


        [Then(@"tinyUrls are created successfully")]
        public async Task ThenTinyUrlsAreCreatedSuccessfully(Table table)
        {
            var tinyUrls = table.CreateSet<TinyUrlDTO>();
            foreach (var tinyUrl in tinyUrls)
            {
                var response = await Client.GetAsync(BaseAddress + tinyUrl.Alias);
                response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            }

        }
        [Given(@"a tinyurl with alias (.*) is created in the system")]
        public async Task GivenATinyurlIsRegisteredInTheSystem(string alias)
        {
            await RegisterAndLogin();

            //clean up
            var deleteRequest = await Client.DeleteAsync("/tinyurl" + alias);

            var response = await Client.PutAsJsonAsync("/tinyurl", new TinyUrlDTO()
            {
                Alias = alias,
                OriginalUrl = "https://www.toolsqa.com/specflow/tables-in-specflow/"
            });
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [When(@"I request a TinyUrl by alias (.*)")]
        public async Task WhenIRequestATinyUrlByAlias(string alias)
        {
            var response = await Client.GetAsync("/tinyurl/get-by-short-form?tinyUrl=" + BaseAddress + alias);
            var content = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TinyUrl>(content);
            actual.OriginalUrl.Should().Be("https://www.toolsqa.com/specflow/tables-in-specflow/");
        }

        [Then(@"If I access the tiny url with alias (.*) I get redirected to my orignal url")]
        public async Task ThenTheResponseWillContainTheOriginalUrl(string alias)
        {
            var response = await Client.GetAsync(BaseAddress + alias);
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        private async Task RegisterAndLogin()
        {
            //check if user already logged in

            var response = await Client.GetAsync("/get-current-user");
            if (response.StatusCode.Equals(HttpStatusCode.OK))
                return;

            //register
            //Arrange
            UserDTO newUser = new UserDTO() { Email = "abc@domain.com", Password = "test", UserName = "abc" };

            //Act
            response = await Client.PutAsJsonAsync("/register", newUser);

            //Assert
            response.StatusCode.Should().BeOneOf(new[] { HttpStatusCode.OK, HttpStatusCode.Conflict });

            //login
            //Arrange
            LoginInfo login = new LoginInfo() { Email = "abc@domain.com", Password = "test" };
            //Act
            await Client.PostAsJsonAsync("/login", login);
        }
    }
}
