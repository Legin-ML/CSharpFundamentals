using Microservice.Models;
using Microservice.Services;
using Moq;
using Xunit;
using Microservice.Data.Repositories;

namespace Microservice.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _service;

        public BookServiceTests()
        {
            _mockRepo = new Mock<IBookRepository>();
            _mockLogger = new Mock<ILogger<BookService>>();
            _service = new BookService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Test Book 2", Author = "Author 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllBooksAsync())
                .ReturnsAsync(books);

            var result = await _service.GetAllBooksAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Id == 1);
            Assert.Contains(result, b => b.Id == 2);
        }

        [Fact]
        public async Task GetBookByIdAsync_WithValidId_ReturnsBook()
        {
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
            _mockRepo.Setup(repo => repo.GetBookByIdAsync(bookId))
                .ReturnsAsync(book);

            var result = await _service.GetBookByIdAsync(bookId);

            Assert.Equal(bookId, result.Id);
        }

        [Fact]
        public async Task GetBookByIdAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            var bookId = 999;
            _mockRepo.Setup(repo => repo.GetBookByIdAsync(bookId))
                .ReturnsAsync((Book)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetBookByIdAsync(bookId));
        }

        [Fact]
        public async Task AddBookAsync_WithValidBook_ReturnsAddedBook()
        {
            var bookRequest = new CreateBookRequest { Title = "New Book", Author = "New Author" };
            var addedBook = new Book { Id = 1, Title = "New Book", Author = "New Author" };

            _mockRepo.Setup(repo => repo.AddBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(addedBook);

            var result = await _service.AddBookAsync(bookRequest);

            Assert.Equal(addedBook.Id, result.Id);
        }

        [Fact]
        public async Task UpdateBookAsync_WithValidIdAndBook_ReturnsUpdatedBook()
        {
            var bookId = 1;
            var updateRequest = new UpdateBookRequest { Title = "Updated Book", Author = "Updated Author" };
            var updatedBook = new Book { Id = bookId, Title = "Updated Book", Author = "Updated Author" };

            _mockRepo.Setup(repo => repo.BookExistsAsync(bookId))
                .ReturnsAsync(true);

            _mockRepo.Setup(repo => repo.UpdateBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(updatedBook);

            var result = await _service.UpdateBookAsync(bookId, updateRequest);

            Assert.Equal(updatedBook.Id, result.Id);
            Assert.Equal("Updated Book", result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            var bookId = 999;
            var updateRequest = new UpdateBookRequest { Title = "Updated Book", Author = "Updated Author" };

            _mockRepo.Setup(repo => repo.BookExistsAsync(bookId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateBookAsync(bookId, updateRequest));
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidId_ReturnsTrue()
        {
            var bookId = 1;

            _mockRepo.Setup(repo => repo.BookExistsAsync(bookId))
                .ReturnsAsync(true);

            _mockRepo.Setup(repo => repo.DeleteBookAsync(bookId))
                .ReturnsAsync(true);

            var result = await _service.DeleteBookAsync(bookId);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteBookAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            var bookId = 999;

            _mockRepo.Setup(repo => repo.BookExistsAsync(bookId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteBookAsync(bookId));
        }
    }
}
