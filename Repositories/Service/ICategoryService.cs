using BusinessObject.Dto;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<CategoryReturnDto?> GetCategoryByIdAsync(short id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(short id);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
}
