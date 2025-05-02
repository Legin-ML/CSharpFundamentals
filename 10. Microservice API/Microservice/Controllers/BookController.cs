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
        public async Task<ActionResult<Book>> CreateBook(CreateBookRequest reqModel)
        {
            _logger.LogInformation("Creating a new book.");

            if (reqModel == null)
            {
                _logger.LogWarning("CreateBook request model is null.");
                return BadRequest("Request model cannot be null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateBook request model is invalid.");
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Author = reqModel.Author,
                Genre = reqModel.Genre,
                Price = reqModel.Price,
                PublishedDate = reqModel.PublishedDate,
                Title = reqModel.Title
            };

            try
            {
                var createdBook = await _bookService.AddBookAsync(reqModel);
                _logger.LogInformation($"Book created successfully with ID: {createdBook.Id}");
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return BadRequest();
            }
        }

        // GET: api/books [READ]
        [HttpGet("/getbooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            _logger.LogInformation("Getting all books");

            try
            {
                var books = await _bookService.GetAllBooksAsync();
                _logger.LogInformation($"Retrieved {books.Count()} books successfully.");
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books");
                return StatusCode(500, "Internal server error");
            }
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
                _logger.LogInformation($"Book with ID: {id} retrieved successfully.");
                return Ok(book);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Book with ID: {id} not found.");
                return NotFound();
            }
        }

        // PUT: api/books/<id> [UPDATE]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookRequest reqModel)
        {
            _logger.LogInformation($"Updating book with ID: {id}");

            if (reqModel == null)
            {
                _logger.LogWarning($"Update request for book with ID {id} is null.");
                return BadRequest("Request cannot be null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Update request for book with ID {id} has invalid data.");
                return BadRequest(ModelState);
            }

            try
            {
                await _bookService.UpdateBookAsync(id, reqModel);
                _logger.LogInformation($"Book with ID {id} updated successfully.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Book with ID {id} not found for update.");
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error updating book with ID {id}");
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/books/<id> [DELETE]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            _logger.LogInformation($"Deleting book with ID: {id}");

            try
            {
                await _bookService.DeleteBookAsync(id);
                _logger.LogInformation($"Book with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Book with ID {id} not found for deletion.");
                return NotFound();
            }
        }
    }
}
