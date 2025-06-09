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
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<NewsArticle, int> _newsArticleRepository;
        private readonly IGenericRepository<Tag, int> _tagRepository;

        public NewsArticleService(IUnitOfWork unitOfWork,
                                  IGenericRepository<NewsArticle, int> newsArticleRepository,
                                  IGenericRepository<Tag, int> tagRepository)
        {
            _unitOfWork = unitOfWork;
            _newsArticleRepository = newsArticleRepository;
            _tagRepository = tagRepository;
        }

        // Get all news articles
        public async Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync()
        {
            return await _newsArticleRepository.FindAll(null, n=> n.Tags, n=> n.Category, n => n.CreatedBy).ToListAsync();
        }

        // Get news article by ID
        public async Task<NewsArticle?> GetNewsArticleByIdAsync(int id)
        {
            return await _newsArticleRepository.FindById(id, "NewsArticleId", a => a.Tags, a => a.Category, a => a.CreatedBy);
        }

        // get news articles by staff ID
        public async Task<IEnumerable<NewsArticle>> GetNewsArticleByStaffIdAsync(short id)
        {
            return await _newsArticleRepository.FindAll(n => n.CreatedById.Equals(id), n => n.Tags, n => n.Category, n => n.CreatedBy).ToListAsync();
        }

        // Add a new news article
        public async Task AddNewsArticleAsync(NewsArticle newsArticle, List<int> tagIds)
        {
            if (newsArticle == null)
                throw new ArgumentNullException(nameof(newsArticle));

            if (tagIds == null || !tagIds.Any())
                throw new ArgumentException("Tag IDs cannot be null or empty.", nameof(tagIds));

            var tags = await _tagRepository.FindAll(t => tagIds.Contains(t.TagId)).ToListAsync();

            if (!tags.Any())
                throw new InvalidOperationException("no valid tag found for the provided tag id");

            newsArticle.Tags = tags;
            _newsArticleRepository.Create(newsArticle);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Update an existing news article
        public async Task UpdateNewsArticleAsync(NewsArticle newsArticle, List<int> tagIds)
        {
            if (newsArticle == null)
                throw new ArgumentNullException(nameof(newsArticle));

            var existingArticle = await _newsArticleRepository.FindById(newsArticle.NewsArticleId, "NewsArticleId", n => n.Tags);
            if (existingArticle == null)
                throw new ArgumentException("News article not found");

            // update the properties of the existing article

            existingArticle.NewsTitle = newsArticle.NewsTitle;
            existingArticle.Headline = newsArticle.Headline;
            existingArticle.NewsContent = newsArticle.NewsContent;
            existingArticle.NewsSource = newsArticle.NewsSource;
            existingArticle.CategoryId = newsArticle.CategoryId;
            existingArticle.NewsStatus = newsArticle.NewsStatus;

            //  update the created by and updated by fields
            existingArticle.UpdatedById = newsArticle.UpdatedById;
            existingArticle.ModifiedDate = DateTime.Now;

            // get the tags based on the provided tag IDs
            var tags = await _tagRepository.FindAll(t => tagIds.Contains(t.TagId)).ToListAsync();

            // update the tags of the existing article
            existingArticle.Tags.Clear();
            existingArticle.Tags = tags;

            _newsArticleRepository.Update(existingArticle);
            await _unitOfWork.SaveChange();
        }

        // Delete a news article by ID
        public async Task DeleteNewsArticleAsync(int id)
        {
            var newsArticle = await _newsArticleRepository.FindById(id, "NewsArticleId", n => n.Tags);
            if (newsArticle == null)
                throw new ArgumentException("News article not found");

            newsArticle.Tags.Clear();
            _newsArticleRepository.Update(newsArticle);
            await _unitOfWork.SaveChange();

            _newsArticleRepository.Delete(newsArticle);
            await _unitOfWork.SaveChange();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsForLecturerAsync()
        {
            return await _newsArticleRepository
                .FindAll(a => a.NewsStatus == true, a => a.Tags, a => a.Category, a => a.CreatedBy)
                .ToListAsync();
        }
    }
}
