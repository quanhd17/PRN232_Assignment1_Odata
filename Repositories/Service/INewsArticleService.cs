using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetNewsArticleByStaffIdAsync(short id);
        Task<NewsArticle?> GetNewsArticleByIdAsync(int id);
        Task AddNewsArticleAsync(NewsArticle newsArticle, List<int> tagIds);
        Task UpdateNewsArticleAsync(NewsArticle newsArticle, List<int> tagIds);
        Task DeleteNewsArticleAsync(int id);
        Task<IEnumerable<NewsArticle>> GetActiveNewsForLecturerAsync();
    }
}
