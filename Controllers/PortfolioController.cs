using System;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFMPService _fmpService;

        public PortfolioController(
            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository,
            IFMPService fmpService
        )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _stockRepository =
                stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
            _portfolioRepository =
                portfolioRepository ?? throw new ArgumentNullException(nameof(portfolioRepository));
            _fmpService = fmpService ?? throw new ArgumentNullException(nameof(fmpService));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {
            try
            {
                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);
                if (appUser == null)
                    return Unauthorized("User not found.");

                var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
                return Ok(userPortfolio);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching portfolio: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the portfolio.");
            }
        }

        [HttpPost("{symbol}")]
        public async Task<IActionResult> AddPortfolio([FromRoute] string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Stock symbol is required.");

            try
            {
                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);
                if (appUser == null)
                    return Unauthorized("User not found.");

                var stock =
                    await _stockRepository.GetBySymbol(symbol)
                    ?? await _fmpService.FindStockBySymbolAsync(symbol);

                if (stock == null)
                    return NotFound("Stock does not exist.");

                if (await _stockRepository.GetBySymbol(symbol) == null)
                    await _stockRepository.CreateAsync(stock);

                var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
                if (
                    userPortfolio.Any(s =>
                        s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)
                    )
                )
                    return BadRequest("The stock already exists in the user's portfolio.");

                var portfolioEntry = new Portfolio { StockId = stock.Id, AppUserId = appUser.Id };
                await _portfolioRepository.CreateAsync(portfolioEntry);

                return CreatedAtAction(
                    nameof(GetUserPortfolio),
                    new { id = portfolioEntry.StockId },
                    portfolioEntry
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding stock to portfolio: {ex.Message}");
                return StatusCode(
                    500,
                    "An error occurred while adding the stock to the portfolio."
                );
            }
        }

        [HttpDelete("{symbol}")]
        public async Task<IActionResult> DeletePortfolio([FromRoute] string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Stock symbol is required.");

            try
            {
                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);
                if (appUser == null)
                    return Unauthorized("User not found.");

                var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
                var portfolioEntry = userPortfolio.FirstOrDefault(s =>
                    s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)
                );

                if (portfolioEntry == null)
                    return NotFound("Stock is not in the user's portfolio.");

                await _portfolioRepository.DeleteAsync(appUser, symbol);
                return Ok($"Stock {symbol} removed from portfolio.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting stock from portfolio: {ex.Message}");
                return StatusCode(
                    500,
                    "An error occurred while removing the stock from the portfolio."
                );
            }
        }
    }
}
