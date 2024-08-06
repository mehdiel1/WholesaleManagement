using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WholesaleManagement.Models;

namespace WholesaleManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class QuotesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public QuotesController(BreweryContext context)
        {
            _context = context;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestQuote([FromBody] QuoteRequest request)
        {
            // Validation
            if (request.BeerOrders == null || !request.BeerOrders.Any())
            {
                return BadRequest("The order cannot be empty.");
            }

            var wholesaler = await _context.Wholesalers.FindAsync(request.WholesalerId);
            if (wholesaler == null)
            {
                return NotFound("The wholesaler must exist.");
            }

            decimal totalPrice = 0;
            var summary = new StringBuilder();
            var errors = new List<string>();

            foreach (var order in request.BeerOrders)
            {
                var beerId = order.Key;
                var quantity = order.Value;

                // Check for duplicate orders
                if (quantity <= 0)
                {
                    errors.Add($"Invalid quantity for beer ID {beerId}. Quantity must be greater than 0.");
                    continue;
                }

                var beer = await _context.Beers.FindAsync(beerId);
                if (beer == null)
                {
                    errors.Add($"Beer ID {beerId} does not exist.");
                    continue;
                }

                var stock = await _context.WholesalerStocks
                                           .Where(ws => ws.WholesalerId == request.WholesalerId && ws.BeerId == beerId)
                                           .SingleOrDefaultAsync();

                if (stock == null)
                {
                    errors.Add($"Beer ID {beerId} is not sold by the wholesaler.");
                    continue;
                }

                if (quantity > stock.Quantity)
                {
                    errors.Add($"Not enough stock for beer ID {beerId}. Available: {stock.Quantity}, Requested: {quantity}.");
                    continue;
                }

                // Calculate price
                decimal price = beer.Price * quantity;
                totalPrice += price;
                summary.AppendLine($"{beer.Name}: {quantity} x {beer.Price:C} = {price:C}");
            }

            if (errors.Any())
            {
                return BadRequest(string.Join(" | ", errors));
            }

            // Apply discounts
            if (request.BeerOrders.Values.Sum() > 20)
            {
                totalPrice *= 0.8m; // 20% discount
            }
            else if (request.BeerOrders.Values.Sum() > 10)
            {
                totalPrice *= 0.9m; // 10% discount
            }

            // Build summary
            summary.AppendLine($"Total Price: {totalPrice:C}");

            var response = new QuoteResponse
            {
                TotalPrice = totalPrice,
                Summary = summary.ToString()
            };

            return Ok(response);
        }
    }


}
