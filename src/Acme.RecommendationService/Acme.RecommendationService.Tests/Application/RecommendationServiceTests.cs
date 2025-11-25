using Acme.RecommendationService.Application.Interfaces;
using Acme.RecommendationService.Application.Services;
using Acme.RecommendationService.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Tests.Application
{
    public class RecommendationServiceTests
    {
        private readonly Mock<IRecommendationRepository> _repoMock;
        private readonly RecommendationServices _service;

        public RecommendationServiceTests()
        {
            _repoMock = new Mock<IRecommendationRepository>();
            _service = new RecommendationServices(_repoMock.Object);
        }

        [Fact]
        public async Task GetForProductAsync_Should_Return_DTO_List()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var entities = new List<Recommendation>
            {
                new Recommendation(Guid.NewGuid(), productId, "Good product!"),
                new Recommendation(Guid.NewGuid(), productId, "Highly recommended!")
            };

            _repoMock
                .Setup(r => r.GetRecommendationsForProductAsync(productId))
                .ReturnsAsync(entities);

            // Act
            var result = await _service.GetForProductAsync(productId);

            // Assert
            result.Should().HaveCount(2);
            result.First().Message.Should().Be("Good product!");
            result.Last().Message.Should().Be("Highly recommended!");

            _repoMock.Verify(r => r.GetRecommendationsForProductAsync(productId), Times.Once);
        }

        [Fact]
        public async Task CreateRecommendationAsync_Should_Create_And_Return_New_Id()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var message = "Excellent choice!";

            Recommendation? addedEntity = null;

            // Capture the recommendation object passed to AddAsync
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Recommendation>()))
                     .Callback<Recommendation>(r => addedEntity = r)
                     .Returns(Task.CompletedTask);

            // Act
            var resultId = await _service.CreateRecommendationAsync(productId, message);

            // Assert
            resultId.Should().NotBeEmpty();
            addedEntity.Should().NotBeNull();

            addedEntity!.ProductId.Should().Be(productId);
            addedEntity.Message.Should().Be(message);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Recommendation>()), Times.Once);
        }
    }
}
