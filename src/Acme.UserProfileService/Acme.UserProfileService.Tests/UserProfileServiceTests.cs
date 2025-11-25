using Acme.UserProfileService.Application.DTOs;
using Acme.UserProfileService.Application.Interfaces;
using Acme.UserProfileService.Application.Services;
using Acme.UserProfileService.Domain;
using FluentAssertions;
using Moq;

namespace Acme.UserProfileService.Tests
{
    public class UserProfileServiceTests
    {
        private readonly Mock<IUserProfileRepository> _mockRepo;
        private readonly UserProfileServices _service;

        public UserProfileServiceTests()
        {
            _mockRepo = new Mock<IUserProfileRepository>();
            _service = new UserProfileServices(_mockRepo.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User_When_Found()
        {
            // Arrange
           
            var user = new UserProfile("Anita", "anita@example.com", 35);
            var id = user.Id;

            _mockRepo.Setup(r => r.GetByIdAsync(id))
                     .ReturnsAsync(user);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Anita");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetByIdAsync(id))
                     .ReturnsAsync((UserProfile?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Users()
        {
            // Arrange
            var users = new List<UserProfile>
            {
                new("Anita", "anita@example.com", 35),
                new("John", "john@example.com", 30)
            };

            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Create_And_Return_Id()
        {
            // Arrange
            var request = new UserProfileDto()
            {
                Name = "Riya",
                Email = "riya@example.com",
                Age = 28
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<UserProfile>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            result.Should().NotBe(Guid.Empty);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<UserProfile>()), Times.Once);
        }
    }
}
