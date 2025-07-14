using Moq;
using SupportSentralFrontEnd.Interfaces;
using SupportSentralFrontEnd.Mappers;
using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontendTest.Mappers
{
    [TestClass]
    public class TicketMapperTests
    {
        private Mock<IUserClient> _mockUserClient;
        private Mock<IStatusClient> _mockStatusClient;
        private TicketMapper _mapper;

        [TestInitialize]
        public void Setup()
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
        public async Task MapFromTicketDetails_WithValidDetails_ReturnsMappedTicket()
        {
            // Arrange
            var ticketDetails = new TicketDetails
            {
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = "1",
                UserId = "user@example.com"
            };

            var expectedUser = new User { Email = "Boateng4@gmail.com", Name = "Test User" };
            _mockUserClient.Setup(x => x.GetUserFromEmail(ticketDetails.UserId))
                          .ReturnsAsync(expectedUser);
            _mockStatusClient.Setup(x => x.GetStatusFromId(ticketDetails.Id))
                           .ReturnsAsync(new Status{Id = 1, Name = "Closed"});

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticketDetails.Title, result.Title);
            Assert.AreEqual(ticketDetails.Description, result.Description);
            Assert.AreEqual(int.Parse(ticketDetails.StatusId), result.StatusId);
            Assert.AreEqual(expectedUser.Name, result.AssignedTo);
            Assert.IsTrue(result.CreatedAt <= DateTime.UtcNow);
        }

        [TestMethod]
        public async Task MapFromTicketDetails_WithNullUserId_ReturnsNull()
        {
            // Arrange
            var ticketDetails = new TicketDetails
            {
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = "1",
                UserId = null
            };

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task MapFromTicketDetails_WithNullStatusId_ReturnsNull()
        {
            // Arrange
            var ticketDetails = new TicketDetails
            {
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = null,
                UserId = "user@example.com"
            };

            var expectedUser = new User { Email = "Boateng84@gmail.com", Name = "Test User" };
            _mockUserClient.Setup(x => x.GetUserFromEmail(ticketDetails.UserId))
                          .ReturnsAsync(expectedUser);

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task MapFromTicketDetails_WhenUserNotFound_ReturnsNull()
        {
            // Arrange
            var ticketDetails = new TicketDetails
            {
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = "1",
                UserId = "user@example.com"
            };

            _mockUserClient.Setup(x => x.GetUserFromEmail(ticketDetails.UserId))
                          .ReturnsAsync((User)null);
            _mockStatusClient.Setup(x => x.GetStatusFromId(ticketDetails.Id))
                           .ReturnsAsync(new Status{Id = 1, Name = "New"});

            // Act
            var result = await _mapper.MapFromTicketDetails(ticketDetails);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task MapToTicketDetails_WithValidTicket_ReturnsMappedDetails()
        {
            // Arrange
            var ticket = new Ticket
            {
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow,
                AssignedTo = "Test User"
            };

            var expectedUser = new User { Email = "user@example.com", Name = "Kwame"};
            _mockUserClient.Setup(x => x.GetUserFromEmail(ticket.AssignedTo))
                          .ReturnsAsync(expectedUser);

            // Act
            var result = await _mapper.MapToTicketsDetails(ticket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.Title, result.Title);
            Assert.AreEqual(ticket.Description, result.Description);
            Assert.AreEqual(ticket.StatusId.ToString(), result.StatusId);
            Assert.AreEqual(ticket.CreatedAt, result.CreatedAt);
            Assert.AreEqual(ticket.UpdatedAt, result.UpdatedAt);
            Assert.AreEqual(expectedUser.Email, result.UserId);
            Assert.IsTrue(result.AssignToSelf);
        }

        [TestMethod]
        public async Task MapToTicketDetails_WithNullTicket_ReturnsNull()
        {
            // Act
            var result = await _mapper.MapToTicketsDetails(null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task MapToTicketDetails_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var ticket = new Ticket
            {
                Title = "Test Ticket",
                Description = "Test Description",
                StatusId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow,
                AssignedTo = "Non-existent User"
            };

            _mockUserClient.Setup(x => x.GetUserFromEmail(ticket.AssignedTo))
                          .ReturnsAsync((User)null);

            // Act
            await _mapper.MapToTicketsDetails(ticket);
        }
    }
}