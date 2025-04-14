using Microservice.Data.Repositories;
using Microservice.Models;
using Microservice.Services;
using Moq;
using Xunit;

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
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Test Book 2", Author = "Author 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllBooksAsync())
                .ReturnsAsync(books);

            // Act
            var result = await _service.GetAllBooksAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Id == 1);
            Assert.Contains(result, b => b.Id == 2);
        }

        [Fact]
        public async Task GetBookByIdAsync_WithValidId_ReturnsBook()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
            _mockRepo.Setup(repo => repo.GetBookByIdAsync(bookId))
                .ReturnsAsync(book);

            // Act
            var result = await _service.GetBookByIdAsync(bookId);

            // Assert
            Assert.Equal(bookId, result.Id);
        }

        [Fact]
        public async Task GetBookByIdAsync_WithInvalidId_ThrowsKeyNotFoundException()  // Testing exception handling
        {
            // Arrange
            var bookId = 999;
            _mockRepo.Setup(repo => repo.GetBookByIdAsync(bookId))
                .ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetBookByIdAsync(bookId));
        }

        [Fact]
        public async Task AddBookAsync_WithValidBook_ReturnsAddedBook()
        {
            // Arrange
            var book = new Book { Title = "New Book", Author = "New Author" };
            var addedBook = new Book { Id = 1, Title = "New Book", Author = "New Author" };

            _mockRepo.Setup(repo => repo.AddBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(addedBook);

            // Act
            var result = await _service.AddBookAsync(book);

            // Assert
            Assert.Equal(addedBook.Id, result.Id);
        }

        [Fact]
        public async Task UpdateBookAsync_WithValidIdAndBook_ReturnsUpdatedBook()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Updated Book", Author = "Updated Author" };

            _mockRepo.Setup(repo => repo.BookExistsAsync(bookId))
                .ReturnsAsync(true);

            _mockRepo.Setup(repo => repo.UpdateBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(book);

            // Act
            var result = await _service.UpdateBookAsync(bookId, book);

            // Assert
            Assert.Equal(bookId, result.Id);
            Assert.Equal("Updated Book", result.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var bookId = 1;

            _mockRepo.Setup(repo => repo.BookExistsAsync(bookId))
                .ReturnsAsync(true);

            _mockRepo.Setup(repo => repo.DeleteBookAsync(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteBookAsync(bookId);

            // Assert
            Assert.True(result);
        }
    }
}
