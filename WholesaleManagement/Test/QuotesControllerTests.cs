using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WholesaleManagement.Controllers;
using WholesaleManagement.Models;

namespace WholesaleManagement.Test
{
    public class QuotesControllerTests
    {
        private readonly QuotesController _controller;
        private readonly Mock<BreweryContext> _mockContext;

        public QuotesControllerTests()
        {
            _mockContext = new Mock<BreweryContext>(new DbContextOptions<BreweryContext>());
            _controller = new QuotesController(_mockContext.Object);
        }

        [Fact]
        public async Task RequestQuote_InvalidWholesaler_ReturnsNotFound()
        {
            // Arrange
            var request = new QuoteRequest
            {
                WholesalerId = 1,
                BeerOrders = new Dictionary<int, int> { { 1, 5 } }
            };
            _mockContext.Setup(c => c.Wholesalers.FindAsync(1)).ReturnsAsync((Wholesaler)null);

            // Act
            var result = await _controller.RequestQuote(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("The wholesaler must exist.", notFoundResult.Value);
        }

        [Fact]
        public async Task RequestQuote_ValidRequest_ReturnsQuote()
        {
            // Arrange
            var wholesaler = new Wholesaler { WholesalerId = 1 };
            var beer = new Beer { BeerId = 1, Name = "Test Beer", Price = 2.00m };
            var stock = new WholesalerStock { WholesalerId = 1, BeerId = 1, Quantity = 15 };

            _mockContext.Setup(c => c.Wholesalers.FindAsync(1)).ReturnsAsync(wholesaler);
            _mockContext.Setup(c => c.Beers.FindAsync(1)).ReturnsAsync(beer);
            _mockContext.Setup(c => c.WholesalerStocks
                                    .Where(ws => ws.WholesalerId == 1 && ws.BeerId == 1))
                        .Returns(new List<WholesalerStock> { stock }.AsQueryable().BuildMock().Object);

            var request = new QuoteRequest
            {
                WholesalerId = 1,
                BeerOrders = new Dictionary<int, int> { { 1, 12 } }
            };

            // Act
            var result = await _controller.RequestQuote(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<QuoteResponse>(okResult.Value);
            Assert.Equal(2.00m * 12 * 0.9m, response.TotalPrice); // 10% discount
            Assert.Contains("Test Beer: 12 x $2.00 = $24.00", response.Summary);
        }
    }

}
