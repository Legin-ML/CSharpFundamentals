using Microservice.Models;

namespace Microservice.Services
{
    public interface IBookService
    {

        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(int id, Book book);
        Task<bool> DeleteBookAsync(int id);

    }
}
