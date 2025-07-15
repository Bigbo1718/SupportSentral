using SupportSentralFrontEnd.Models;
using SupportSentralFrontEnd.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using SupportSentralFrontEnd.Mappers;

namespace SupportSentralFrontEnd.Tests.Mappers
{
    [TestClass]
    public class TicketMapperTests
    {
        private Mock<IUserClient> _mockUserClient;
        private Mock<IStatusClient> _mockStatusClient;
        private TicketMapper _mapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUserClient = new Mock<IUserClient>();
            _mockStatusClient = new Mock<IStatusClient>();
            _mapper = new TicketMapper
            {
                _userClient = _mockUserClient.Object,
                _statusClient = _mockStatusClient.Object
            };
        }

        [TestMethod]
        public async Task MapFromTicketDetails_WithValidDetails_ReturnsTicket()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticketDetails = new TicketDetails
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = 1,
                UserId = userId,
            };

            var expectedUser = new User 
            { 
                Id = userId,
                Name = "Test User",
                Email = "test@example.com"
            };

            var expectedStatus = new Status { Id = 1, Name = "Open" };

            _mockUserClient.Setup(x => x.GetUserFromId(userId))
                          .ReturnsAsync(expectedUser);
            _mockStatusClient.Setup(x => x.GetStatusFromId(ticketDetails.StatusId))
                           .ReturnsAsync(expectedStatus);

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticketDetails.Title, result.Title);
            Assert.AreEqual(ticketDetails.Description, result.Description);
            Assert.AreEqual(ticketDetails.StatusId, result.StatusId);
            Assert.AreEqual(expectedUser.Name, result.AssignedTo);
            Assert.AreEqual(expectedUser.Id, result.UserId); 
        }

        [TestMethod]
        public async Task MapFromTicketDetails_WithNullUserId_ReturnsNull()
        {
            // Arrange
            var ticketDetails = new TicketDetails
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = 1,
                UserId = null
            };

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task MapFromTicketDetails_WithInvalidStatusId_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticketDetails = new TicketDetails
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = null,
                UserId = userId
            };

            var expectedUser = new User 
            { 
                Id = userId,
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserClient.Setup(x => x.GetUserFromId(userId))
                          .ReturnsAsync(expectedUser);

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task MapToTicketDetails_WithValidTicket_ReturnsTicketDetails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = 1,
                UserId = userId,
                AssignedTo = "test@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };

            var expectedUser = new User 
            { 
                Id = userId,
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserClient.Setup(x => x.GetUserFromEmail(ticket.AssignedTo))
                          .ReturnsAsync(expectedUser);

            // Act
            var result = await _mapper.MapToTicketsDetails(ticket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.Id, result.Id);
            Assert.AreEqual(ticket.Title, result.Title);
            Assert.AreEqual(ticket.Description, result.Description);
            Assert.AreEqual(ticket.StatusId, result.StatusId);
            Assert.AreEqual(ticket.CreatedAt, result.CreatedAt);
            Assert.AreEqual(ticket.UpdatedAt, result.UpdatedAt);
            Assert.AreEqual(ticket.UserId, result.UserId);
            Assert.IsTrue(result.AssignToSelf);
        }

        [TestMethod]
        public async Task MapToTicketDetails_WithNullUpdatedAt_PreservesNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = 1,
                UserId = userId,
                AssignedTo = "test@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            var expectedUser = new User 
            { 
                Id = userId,
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserClient.Setup(x => x.GetUserFromEmail(ticket.AssignedTo))
                          .ReturnsAsync(expectedUser);

            // Act
            var result = await _mapper.MapToTicketsDetails(ticket);

            // Assert
            Assert.IsNull(result.UpdatedAt);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task MapToTicketDetails_WithNullTicket_ThrowsException()
        {
            // Arrange
            Ticket ticket = null;

            // Act
            await _mapper.MapToTicketsDetails(ticket);
        }
    }
}