using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api.IntegrationTests;

public class TicketRequestTest
{
    [Fact]
    public async Task TicketRequest_PostRequestWithValidParameters_ShouldReturnNewTicketObj()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();

        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var response = await httpClient.PostAsync("/tickets", content);


        response.EnsureSuccessStatusCode();
        
        var matchResponse = await response.Content.ReadFromJsonAsync<TicketDetailsContract>();
        matchResponse?.Id.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task TicketRequest_PostRequestWithoutTitle_ShouldReturnErrorMessage404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();

        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };

        var ticketObj =
        new {
            Description = "The light is Low",
            StatusId = 1,
            UserId =  $"{loggedInUser.Id}"
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");
        
        var response = await httpClient.PostAsync("/tickets", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }

    [Fact]
    public async Task TicketRequest_PostRequestWithoutDescription_ShouldReturnErrorMessage404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();

        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };

        var ticketObj =
            new {
                Title = "Where is Our Descipt",
                StatusId = 1,
                UserId =  $"{loggedInUser.Id}"
            };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");
        
        var response = await httpClient.PostAsync("/tickets", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_PostRequestWithoutStatus_ShouldReturnErrorMessage404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();

        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };

        var ticketObj =
            new {
                Title = "Where is Our Descipt",
                Description = "The light is Low",
                UserId =  $"{loggedInUser.Id}"
            };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");
        
        var response = await httpClient.PostAsync("/tickets", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_PostRequestWithoutUser_ShouldReturnErrorMessage404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        
        var ticketObj =
            new {
                Title = "Where is Our Descipt",
                Description = "The light is Low",
                StatusId = 1,
            };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");
        
        var response = await httpClient.PostAsync("/tickets", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_GetRequestToGetAllTicket_ShouldReturnListOfTickets()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        
        var response = await httpClient.GetAsync("/tickets");

        response.EnsureSuccessStatusCode();
        
        var matchResponse = await response.Content.ReadFromJsonAsync<List<TicketDetailsContract>>();
        matchResponse.Should().NotBeNull();
        
    }
    [Fact]
    public async Task TicketRequest_GetRequestWithId_ShouldReturnTicketDetails()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
        var matchPostResponse = await postResponse.Content.ReadFromJsonAsync<TicketDetailsContract>();
        
        var response = await httpClient.GetAsync($"/tickets/{matchPostResponse?.Id}");

        response.EnsureSuccessStatusCode();
        
        var matchResponse = await response.Content.ReadFromJsonAsync<TicketDetailsContract>();
        matchResponse?.Title.Should().Be("Light Went Out");
        matchResponse?.Description.Should().Be("the light is Low");
        matchResponse?.StatusId.Should().Be(1);
        matchResponse?.UserId.Should().Be(loggedInUser.Id);
    }
    [Fact]
    public async Task TicketRequest_GetRequestWithInvalidId_ShouldReturnError400()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
    
        var response = await httpClient.GetAsync($"/tickets/{Guid.NewGuid()}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    [Fact]
    public async Task TicketRequest_PuTRequestWithValidParameters_ShouldReturnUpdatedTicketObj()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
        var matchPostResponse = await postResponse.Content.ReadFromJsonAsync<TicketDetailsContract>();
        
        Ticket updatedTicket = new Ticket()
        {
            Title = "Light is Even Lower",
            Description = "the light is Low",
            StatusId = 2,
            UserId = Guid.NewGuid()
        };
        HttpContent updatedContent = new StringContent(JsonConvert.SerializeObject(updatedTicket),
            Encoding.UTF8, 
            "application/json");

        var response = await httpClient.PutAsync($"/tickets/{matchPostResponse?.Id}",updatedContent );

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_PutRequestWithInvalidId_ShouldReturnUpdatedTicketObj()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
        var matchPostResponse = await postResponse.Content.ReadFromJsonAsync<TicketDetailsContract>();
        
        Ticket updatedTicket = new Ticket()
        {
            Title = "Light is Even Lower",
            Description = "the light is Low",
            StatusId = 2,
            UserId = Guid.NewGuid()
        };
        HttpContent updatedContent = new StringContent(JsonConvert.SerializeObject(updatedTicket),
            Encoding.UTF8, 
            "application/json");

        var response = await httpClient.PutAsync($"/tickets/{Guid.NewGuid()}",updatedContent );
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_PutRequestWithMissingTitle_ShouldReturnError404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
        var matchPostResponse = await postResponse.Content.ReadFromJsonAsync<TicketDetailsContract>();
        
        var ticketObj =
            new {
                Description = "The light is Low",
                StatusId = 1,
                UserId =  $"{loggedInUser.Id}"
            };

        HttpContent updatedContent = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");

        var response = await httpClient.PutAsync($"/tickets/{matchPostResponse?.Id}",updatedContent );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_PutRequestWithMissingDescription_ShouldReturnError404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
        var matchPostResponse = await postResponse.Content.ReadFromJsonAsync<TicketDetailsContract>();
        
        var ticketObj =
            new {
                Title = "Where My Friend Descript",
                StatusId = 1,
                UserId =  $"{loggedInUser.Id}"
            };

        HttpContent updatedContent = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");

        var response = await httpClient.PutAsync($"/tickets/{matchPostResponse?.Id}",updatedContent );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
    [Fact]
    public async Task TicketRequest_PutRequestWithMissingStatus_ShouldReturnError404()
    {
        var app = new SupportSentralWebApplicationFactory();
        var httpClient = app.CreateClient();
        User loggedInUser = new User()
        {
            Id = new Guid("FCCBA9A0-0588-4D1E-BC46-1E380F5CA903"),
            Name = "John Doe",
            Email = "Boateng84@Tested.om"
        };
        
        Ticket testTicket = new Ticket()
        {
            Title = "Light Went Out",
            Description = "the light is Low",
            StatusId = 1,
            UserId = loggedInUser.Id
        };

        HttpContent content = new StringContent(JsonConvert.SerializeObject(testTicket),
            Encoding.UTF8, 
            "application/json");
        
        var postResponse = await httpClient.PostAsync("/tickets", content);

        postResponse.EnsureSuccessStatusCode();
        var matchPostResponse = await postResponse.Content.ReadFromJsonAsync<TicketDetailsContract>();
        
        var ticketObj =
            new {
                Title = "Where My Friend Descript",
                Description= "Here I am, but now where is status ",
                UserId =  $"{loggedInUser.Id}"
            };

        HttpContent updatedContent = new StringContent(JsonConvert.SerializeObject(ticketObj),
            Encoding.UTF8, 
            "application/json");

        var response = await httpClient.PutAsync($"/tickets/{matchPostResponse?.Id}",updatedContent );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }

}