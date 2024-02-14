using AutoMapper;
using BooksAP.Interfcae;
using BooksAPI.DTOs.CategoryDtos;
using BooksAPI.DTOs.Entities;
using BooksAPI.DTOs.Interface;

namespace BooksAP.Services;
public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task AddCategory(AddCategoryDto categoryDto)
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        var category = _mapper.Map<Category>(categoryDto);
        if (categoryDto is null)
        {
            throw new Exception("Category was null!");
        }

        if (string.IsNullOrEmpty(categoryDto.CategoryName))
        {
            throw new Exception("Category name is required!");
        }

        if (list.Any(c => c.CategoryName == categoryDto.CategoryName))
        {
            throw new Exception($"{categoryDto.CategoryName} name is already exist!");
        }
        await _unitOfWork.CategoryInterface.AddAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategory(int id)
    {
        _unitOfWork.CategoryInterface.Delete(id);
        await _unitOfWork.SaveAsync();
    }
    public async Task<List<CategoryDto>> GetAllCategories()
    {
        var categories = await _unitOfWork.CategoryInterface.GetAllCategoriesWithBooksAsync();
        return categories.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
    }
    public async Task<CategoryDto> GetCategoryById(int id)
    {
        var category = await _unitOfWork.CategoryInterface.GetByIdAsync(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task UpdateCategory(CategoryDto update)
    {

        var categories = await _unitOfWork.CategoryInterface.GetAllAsync();
        var category = _mapper.Map<Category>(update);
        if (update == null)
            throw new ArgumentNullException();
        if (string.IsNullOrEmpty(update.CategoryName))
            throw new ArgumentNullException();
        if (categories.Any(c => c.CategoryName == update.CategoryName))
        {
            throw new ArgumentNullException($"{update.CategoryName} is already exist");
        }
        _unitOfWork.CategoryInterface.Update(category);
        await _unitOfWork.SaveAsync();
    }
}