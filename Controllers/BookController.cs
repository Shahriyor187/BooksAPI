using BooksAP.Interfcae;
using BooksAPI.DTOs.BookDtos;
using BooksAPI.DTOs.CategoryDtos;
using BooksAPI.DTOs.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BooksAP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController(IBookService bookService) : ControllerBase
    {
       private readonly IBookService _bookservice = bookService;
       [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookservice.GetAllBook();
            return Ok(books);
        }
        [HttpGet("Id")]
       public async Task<IActionResult> GetById(int id)
       {
            try
            {
                var book = await _bookservice.GetBookById(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
       }
       [HttpPost]
       public async Task<IActionResult> Add(AddBookDto addbook)
       {
            try
            {
                await _bookservice.AddBook(addbook);
                return Ok("Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
       }
        [HttpPut]
        public async Task<IActionResult> Update(BookDto bookDto)
        {
            try
            {
                await _bookservice.UpdateBook(bookDto);
                return Ok("Updated");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookservice.DeleteBook(id);
                return Ok("Deleted");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}