using System;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;

        public CommentController(
            ICommentRepository commentRepository,
            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IFMPService fmpService
        )
        {
            _commentRepository =
                commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _stockRepository =
                stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _fmpService = fmpService ?? throw new ArgumentNullException(nameof(fmpService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var comments = await _commentRepository.GetAllAsync(query);
                var commentDtos = comments.Select(comment => comment.ToCommentDto());
                return Ok(commentDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching comments: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching comments.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] CommentQueryObject query)
        {
            try
            {
                var comment = await _commentRepository.GetByIdAsync(id, query);
                if (comment == null)
                {
                    return NotFound($"Comment with ID {id} not found.");
                }

                return Ok(comment.ToCommentDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching comment: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the comment.");
            }
        }

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> Create(
            string symbol,
            [FromBody] CreateCommentDto commentDto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var stock =
                    await _stockRepository.GetBySymbol(symbol)
                    ?? await _fmpService.FindStockBySymbolAsync(symbol);

                if (stock == null)
                    return BadRequest("The stock does not exist.");

                if (await _stockRepository.GetBySymbol(symbol) == null)
                    await _stockRepository.CreateAsync(stock);

                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);

                if (appUser == null)
                    return Unauthorized("User not found.");

                var comment = commentDto.ToCommentFromCreate(stock.Id);
                comment.AppUserId = appUser.Id;

                await _commentRepository.CreateAsync(comment);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = comment.Id },
                    comment.ToCommentDto()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating comment: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the comment.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCommentRequestDto commentDto,
            [FromQuery] CommentQueryObject query
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedComment = await _commentRepository.UpdateAsync(
                    id,
                    commentDto.ToCommentFromUpdate(),
                    query
                );

                if (updatedComment == null)
                    return NotFound($"Comment with ID {id} not found.");

                return Ok(updatedComment.ToCommentDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating comment: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the comment.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedComment = await _commentRepository.DeleteAsync(id);

                if (deletedComment == null)
                    return NotFound($"Comment with ID {id} not found.");

                return Ok(deletedComment.ToCommentDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting comment: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the comment.");
            }
        }
    }
}
