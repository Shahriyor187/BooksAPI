using BooksAPI.DTOs.CategoryDtos;

namespace BooksAP.Interfcae;
public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategories();
    Task<CategoryDto> GetCategoryById(int id);
    Task AddCategory(AddCategoryDto categoryDto);
    Task UpdateCategory(CategoryDto update);
    Task DeleteCategory(int id);
}
