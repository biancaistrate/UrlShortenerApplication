using Domain.Master;
using FluentAssertions;
using IntegrationTestProject;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Identity.Client;
using System.Net;
using WebApi.Models;

namespace TestProject1
{
    public class TinyUrlTests :
    IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;


        public TinyUrlTests(
            CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async void CreateNewTinyUri()
        {
            //register
            //Arrange
            UserDTO newUser = new UserDTO() { Email = "abc@domain.com", Password = "test", UserName="abc" };

            //Act
            var response = await _client.PutAsJsonAsync("/register", newUser);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //login
            //Arrange
            LoginInfo login = new LoginInfo() { Email = "abc@domain.com", Password = "test" };

            //Act
            await _client.PostAsJsonAsync("/login", login);
            var currentAccount = await _client.GetFromJsonAsync("/get-current-user", typeof(UserIdentity));

            //Assert
            currentAccount.Should().BeOfType<UserIdentity>().Subject.Email.Equals("abc@domain.com");

            // Arrange
            TinyUrlDTO tinyUrlDTO = new TinyUrlDTO() { Alias = "tinyalias", OriginalUrl = "http://domelongurlthatgoesnowhere" };

            //create new tiny url
            //Act
            response = await _client.PutAsJsonAsync("/tinyurl", tinyUrlDTO);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            //test redirect
            // Arrange
            //Act
            response = await _client.GetAsync("/tinyalias");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }
    }
}