using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        private IQueryable<Comment> GetCommentsQuery()
        {
            return _context.Comments.Include(a => a.AppUser).AsQueryable();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await GetCommentsQuery().ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await GetCommentsQuery().FirstOrDefaultAsync(comment => comment.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var comment = await GetCommentsQuery().FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            comment.Title = commentModel.Title;
            comment.Content = commentModel.Content;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return null;
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
