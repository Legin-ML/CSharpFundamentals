using Microservice.Models;
using Microservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }


        // POST: api/books [CREATE]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdBook = await _bookService.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return BadRequest();
            }
        }

        // GET: api/books [READ]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            _logger.LogInformation("Getting all books");
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // GET: api/books/<id> [READ]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            _logger.LogInformation($"Getting book with ID: {id}");
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                return Ok(book);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        

        // PUT: api/books/<id> [UPDATE]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bookService.UpdateBookAsync(id, book);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/books/<id> [DELETE]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

