using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId);
        Task<Comment> AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        Task<Comment?> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
    }
}
