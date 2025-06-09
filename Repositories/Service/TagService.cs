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
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Tag, int> _tagRepository;

        public TagService(IUnitOfWork unitOfWork, IGenericRepository<Tag, int> tagRepository)
        {
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
        }

        // Get all tags
        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.FindAll().ToListAsync();
        }

        // Get tag by ID
        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _tagRepository.FindById(id, "TagId");
        }

        // Add a new tag
        public async Task AddTagAsync(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _tagRepository.Create(tag);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Update an existing tag
        public async Task UpdateTagAsync(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _tagRepository.Update(tag);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Delete a tag by ID
        public async Task DeleteTagAsync(int id)
        {
            var tag = await _tagRepository.FindById(id, "TagId", t => t.NewsArticles);
            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            if (tag.NewsArticles != null && tag.NewsArticles.Any())
                throw new InvalidOperationException("Cannot delete a tag that is associated with news articles.");
            _tagRepository.Delete(tag);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }
    }
}
