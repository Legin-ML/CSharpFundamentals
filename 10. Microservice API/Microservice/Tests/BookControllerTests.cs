using Microservice.Controllers;
using Microservice.Models;
using Microservice.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Microservice.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly Mock<ILogger<BooksController>> _mockLogger;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _mockLogger = new Mock<ILogger<BooksController>>();
            _controller = new BooksController(_mockBookService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetBooks_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Test Book 2", Author = "Author 2" }
            };
            _mockBookService.Setup(service => service.GetAllBooksAsync())
                .ReturnsAsync(books);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Equal(2, returnedBooks.Count());
        }

        [Fact]
        public async Task GetBook_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
            _mockBookService.Setup(service => service.GetBookByIdAsync(bookId))
                .ReturnsAsync(book);

            // Act
            var result = await _controller.GetBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(bookId, returnedBook.Id);
        }

        [Fact]
        public async Task GetBook_WithInvalidId_ReturnsNotFound() // Testing exception handling
        {
            // Arrange
            var bookId = 999;
            _mockBookService.Setup(service => service.GetBookByIdAsync(bookId))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetBook(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateBook_WithValidBook_ReturnsCreatedAtAction()
        {
            // Arrange
            var book = new Book { Title = "New Book", Author = "New Author" };
            var createdBook = new Book { Id = 1, Title = "New Book", Author = "New Author" };

            _mockBookService.Setup(service => service.AddBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(createdBook);

            // Act
            var result = await _controller.CreateBook(book);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(createdAtActionResult.Value);
            Assert.Equal(createdBook.Id, returnedBook.Id);
            Assert.Equal("GetBook", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task UpdateBook_WithValidIdAndBook_ReturnsNoContent()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Updated Book", Author = "Updated Author" };

            _mockBookService.Setup(service => service.UpdateBookAsync(bookId, It.IsAny<Book>()))
                .ReturnsAsync(book);

            // Act
            var result = await _controller.UpdateBook(bookId, book);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var bookId = 1;

            _mockBookService.Setup(service => service.DeleteBookAsync(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
