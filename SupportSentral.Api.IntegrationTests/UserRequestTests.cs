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

//TODO: Mock DbInstance and Validate Input that has been added to database

public class UserRequestTests
{
    [Fact]
    public async Task UserRequest_PostRequest_ShouldReturnNewUserObj()
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
    public async Task UserRequest_PostRequestWithoutEmail_ShouldReturnError400()
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
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task UserRequest_PostRequestInvalid_ShouldReturnError400()
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
    public async Task UserRequest_GetRequestWithEmail_ShouldReturnUser()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
            Email = "Boateng84@live.com",
            Name = "Boateng B",
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var postResponse = await client.PostAsync("/users", content);

        var userResponse = await postResponse.Content.ReadFromJsonAsync<UserContract>();
        
        var getResponse = await client.GetAsync($"/users/{user.Email}");
        getResponse.EnsureSuccessStatusCode();
        
        var userGetResponse = await getResponse.Content.ReadFromJsonAsync<UserContract>();
        userGetResponse?.Email.Should().Be(user.Email);
        ;

    }
    
    [Fact]
    public async Task UserRequest_GetRequestNullValues_ShouldReturnError400()
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
    [Fact]
    public async Task UserRequest_GetRequestWithoutEmail_ShouldReturnError400()
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

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UserRequest_PutRequestWithValidValue_ShouldReturn204()
    {
        var app = new SupportSentralWebApplicationFactory();
        var client = app.CreateClient();
        User user = new User
        {
            Email = "Boateng84@live.com",
            Name = "Boateng B",
        };
        HttpContent content = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8, 
            "application/json");

        
        var postResponse = await client.PostAsync("/users", content);
        
        postResponse.EnsureSuccessStatusCode();
        
        var returnedUser = await postResponse.Content.ReadFromJsonAsync<UserContract>();
        var userId = returnedUser?.Id;
        User updateUser = new User
        {
            Email = "Boateng84@live.com",
            Name = "Boateng C",
        };
        HttpContent putContent = new StringContent(JsonConvert.SerializeObject(updateUser),
            Encoding.UTF8, 
            "application/json");
        
        var putResponse = await client.PutAsync($"/users/{userId}", putContent);
        putResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        
    }
}
