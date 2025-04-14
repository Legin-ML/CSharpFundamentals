using Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(BookDbContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _logger.LogInformation($"Adding a new Book: {book.Title}");
            _context.Books.Add( book );
            await _context.SaveChangesAsync();
            return book;
            
        }

        public async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            _logger.LogInformation($"Deleting Book: {id}");
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            _logger.LogInformation("Getting all books");
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id) 
        {
            _logger.LogInformation($"Getting book with ID: {id}");
            return await _context.Books.FindAsync(id);
            
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            _logger.LogInformation($"Updating book with ID: {book.Id}");
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
