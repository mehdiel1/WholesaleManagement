using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WholesaleManagement.Controllers;
using WholesaleManagement.Models;

namespace WholesaleManagement.Test
{
    public class BeerControllerTests
    {
        private readonly BeersController _controller;
        private readonly Mock<IBreweryService> _mockService;

        public BeerControllerTests()
        {
            _mockService = new Mock<IBreweryService>();
            _controller = new BeersController(_mockService.Object);
        }

        [Fact]
        public async Task GetBeers_ReturnsOkResult_WithListOfBeers()
        {
            // Arrange
            _mockService.Setup(service => service.GetBeers())
                        .ReturnsAsync(new List<Beer> { new Beer { Name = "Test Beer" } });

            // Act
            var result = await _controller.GetBeers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var beers = Assert.IsType<List<Beer>>(okResult.Value);
            Assert.Single(beers);
        }
    }

}
