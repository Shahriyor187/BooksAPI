using AutoMapper;
using BooksAP.Interfcae;
using BooksAPI.DTOs.BookDtos;
using BooksAPI.DTOs.Entities;
using BooksAPI.DTOs.Interface;

namespace BooksAP.Services;

public class BookService(IUnitOfWork unitOfWork, IMapper mapper) : IBookService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task AddBook(AddBookDto book)
    {
        if (book == null)
        {
            throw new ArgumentNullException(nameof(book));
        }
        if (string.IsNullOrEmpty(book.Title))
        {
            throw new ArgumentNullException(nameof(book));
        }
        var book1 = _mapper.Map<Book>(book);
        await _unitOfWork.BookInterface.AddAsync(book1);
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteBook(int id)
    {
        var book = await _unitOfWork.BookInterface.GetByIdAsync(id);
        if (book != null)
        {
            _unitOfWork.BookInterface.Delete(book.Id);
            await _unitOfWork.SaveAsync();
        }
    }
    public async Task<List<BookDto>> GetAllBook()
    {
        var books = await _unitOfWork.BookInterface.GetBooksWithCategoryAsync();
        return books.Select(b => _mapper.Map<BookDto>(b)).ToList();
    }

    public async Task<BookDto> GetBookById(int id)
    {
        var book = await _unitOfWork.BookInterface.GetByIdAsync(id);
        return _mapper.Map<BookDto>(book);
    }
    public async Task UpdateBook(BookDto book)
    {
        if (book == null)
        {
            throw new ArgumentNullException(nameof(book));
        }
        var book1 = _mapper.Map<Book>(book);
        book1.Category = null;
        _unitOfWork.BookInterface.Update(book1);
        await _unitOfWork.SaveAsync();
    }
}