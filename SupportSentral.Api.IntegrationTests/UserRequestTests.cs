using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api.IntegrationTests;

public class UserRequestTests
{
    [Fact]
    public async Task UserRequest_CreateUser_ShouldReturnNewUserObj()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
            Email = "Boateng84@gmail.com",
            Name = "Boateng B",
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var response = await client.PostAsync("/users", content);
        
        response.EnsureSuccessStatusCode();

        var userResponse = await response.Content.ReadFromJsonAsync<UserContract>();
        Assert.NotNull(userResponse);
        userResponse.Id.Should().NotBeEmpty();
        userResponse.Email.Should().Be(user.Email);
    }
    [Fact]
    public async Task UserRequest_CreateUserWithoutEmail_ShouldReturnError400()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
            Name = "Boateng B",
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var response = await client.PostAsync("/users", content);

        var userResponse = await response.Content.ReadFromJsonAsync<UserContract>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task UserRequest_CreateUserInvalid_ShouldReturnError400()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
            Email = "Bpo.com",
            Name = "Boateng B",
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var response = await client.PostAsync("/users", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UserRequest_GetUserWithEmail_ShouldReturnUser()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
            Name = "Boateng B",
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var response = await client.PostAsync("/users", content);

        var userResponse = await response.Content.ReadFromJsonAsync<UserContract>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UserRequest_GetUserNullValues_ShouldReturnError400()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var response = await client.PostAsync("/users", content);

        var userResponse = await response.Content.ReadFromJsonAsync<UserContract>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
