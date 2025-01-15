using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(
            ApplicationDBContext context,
            ICommentRepository commentRepository,
            UserManager<AppUser> userManager,
            IStockRepository stockRepository
        )
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var CommentDto = comments.Select(comment => comment.ToCommentDto());
            return Ok(CommentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create(
            [FromRoute] int stockId,
            CreateCommentDto commentDto
        )
        {
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username); 
            var comment = commentDto.ToCommentFromCreate(stockId);
            comment.AppUserId = appUser.Id;
            await _commentRepository.CreateAsync(comment);
            return CreatedAtAction(
                nameof(GetById),
                new { id = comment.Id },
                comment.ToCommentDto()
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            UpdateCommentRequestDto commentDto
        )
        {
            var comment = commentDto.ToCommentFromUpdate();
            var updatedComment = await _commentRepository.UpdateAsync(id, comment);
            if (updatedComment == null)
            {
                return NotFound();
            }
            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
    }
}
