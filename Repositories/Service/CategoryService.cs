using BusinessLogic.UnitOfWork;
using BusinessObject.Dto;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Category, short> _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork, IGenericRepository<Category, short> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        // Get all categories
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.FindAll().ToListAsync();
        }

        // Get category by ID
        public async Task<CategoryReturnDto?> GetCategoryByIdAsync(short id)
        {
            var category = await _categoryRepository.FindById(id, "CategoryId", c => c.ParentCategory);
            return MapToDto(category);
        }

        // Add a new category
        public async Task AddCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _categoryRepository.Create(category);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Update an existing category
        public async Task UpdateCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Delete a category by ID
        public async Task DeleteCategoryAsync(short id)
        {
            var category = await _categoryRepository.FindById(id, "CategoryId", c => c.NewsArticles);
            if (category == null)
                throw new ArgumentException("Category not found");
            if (category.NewsArticles != null && category.NewsArticles.Any())
            {
                Console.WriteLine("Category has associated articles.");
                throw new InvalidOperationException("Cannot delete this category because it is associated with one or more news articles.");
            }

            _categoryRepository.Delete(category);
            await _unitOfWork.SaveChange();
        }

        // get active categories
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.FindAll(c => c.IsActive == true).ToListAsync();
        }

        // map to DTO
        public static CategoryReturnDto MapToDto(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            return new CategoryReturnDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive,
                ParentCategory = category.ParentCategory == null ? null : new ParentCategoryDto
                {
                    CategoryId = category.ParentCategory.CategoryId,
                    CategoryName = category.ParentCategory.CategoryName,
                    CategoryDesciption = category.ParentCategory.CategoryDesciption,
                    ParentCategoryId = category.ParentCategory.ParentCategoryId,
                    IsActive = category.ParentCategory.IsActive
                }
            };
        }
    }
}
