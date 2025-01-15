using System;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            try
            {
                var stocks = await _stockRepository.GetAllAsync(query);
                if (!stocks.Any())
                    return NotFound("No stocks found.");

                var stockDtos = stocks.Select(stock => stock.ToStockDto()).ToList();
                return Ok(stockDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching stocks: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching stocks.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var stock = await _stockRepository.GetByIdAsync(id);
                if (stock == null)
                    return NotFound($"Stock with ID {id} not found.");

                return Ok(stock.ToStockDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching stock: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the stock.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var stock = stockDto.toStockFromCreateDTO();
                await _stockRepository.CreateAsync(stock);
                return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating stock: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the stock.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedStock = await _stockRepository.UpdateAsync(id, stockDto);
                if (updatedStock == null)
                    return NotFound($"Stock with ID {id} not found.");

                return Ok(updatedStock.ToStockDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the stock.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedStock = await _stockRepository.DeleteAsync(id);
                if (deletedStock == null)
                    return NotFound($"Stock with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting stock: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the stock.");
            }
        }
    }
}
