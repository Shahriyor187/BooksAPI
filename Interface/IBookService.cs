using BooksAPI.DTOs.BookDtos;

namespace BooksAP.Interfcae;
public interface IBookService
{
    Task<List<BookDto>> GetAllBook();
    Task<BookDto> GetBookById(int id);
    Task AddBook(AddBookDto book);
    Task UpdateBook(BookDto book);
    Task DeleteBook(int id);
}