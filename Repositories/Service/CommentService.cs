using BusinessLogic.UnitOfWork;
using BusinessObject.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Comment, int> _commentRepository;

        public CommentService(IUnitOfWork unitOfWork, IGenericRepository<Comment, int> commentRepository)
        {
            _unitOfWork = unitOfWork;
            _commentRepository = commentRepository;
        }

        // Get comments by article ID
        public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId)
        {
            return await _commentRepository
                .FindAll(c => c.NewsArticleId == articleId && c.IsActive == true, c => c.Account)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        // Add a new comment
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            // For anonymous comments, we need to use a default account ID
            // You might want to create a special "Anonymous" account in your database
            if (comment.AccountId <= 0)
            {
                // Use account ID 1 as default for anonymous comments
                // You should create an "Anonymous" account with ID 1 in your database
                comment.AccountId = 1;
            }

            comment.CreatedDate = DateTime.Now;
            comment.IsActive = true;

            _commentRepository.Create(comment);
            await _unitOfWork.SaveChange();

            return comment;
        }

        // Update an existing comment
        public async Task UpdateCommentAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            _commentRepository.Update(comment);
            await _unitOfWork.SaveChange();
        }

        // Delete a comment by ID
        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _commentRepository.FindById(commentId, "CommentId");
            if (comment == null)
                throw new ArgumentException("Comment not found");

            _commentRepository.Delete(comment);
            await _unitOfWork.SaveChange();
        }

        // Get comment by ID
        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await _commentRepository.FindById(commentId, "CommentId", c => c.Account, c => c.NewsArticle);
        }

        // Get all comments
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _commentRepository
                .FindAll(c => c.IsActive == true, c => c.Account, c => c.NewsArticle)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}
