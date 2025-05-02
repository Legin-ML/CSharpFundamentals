using Microservice.Controllers;
using Microservice.Models;
using Microservice.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Text.Json;

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
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Test Book 2", Author = "Author 2" }
            };
            _mockBookService.Setup(service => service.GetAllBooksAsync())
                .ReturnsAsync(books);

            var result = await _controller.GetBooks();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Equal(2, returnedBooks.Count());
        }

        [Fact]
        public async Task GetBook_WithValidId_ReturnsOkResult()
        {
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
            _mockBookService.Setup(service => service.GetBookByIdAsync(bookId))
                .ReturnsAsync(book);

            var result = await _controller.GetBook(bookId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(bookId, returnedBook.Id);
        }

        [Fact]
        public async Task GetBook_WithInvalidId_ReturnsNotFound()
        {
            var bookId = 999;
            _mockBookService.Setup(service => service.GetBookByIdAsync(bookId))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.GetBook(bookId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateBook_WithValidBook_ReturnsCreatedAtAction()
        {
            var book = new CreateBookRequest { Title = "New Book", Author = "New Author" };
            var createdBook = new Book { Id = 1, Title = "New Book", Author = "New Author" };

            _mockBookService.Setup(service => service.AddBookAsync(It.IsAny<CreateBookRequest>()))
                .ReturnsAsync(createdBook);

            var result = await _controller.CreateBook(book);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(createdAtActionResult.Value);
            Assert.Equal(createdBook.Id, returnedBook.Id);
            Assert.Equal("GetBook", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task CreateBook_WithNullRequest_ReturnsBadRequest()
        {
            var result = await _controller.CreateBook(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_WithValidIdAndBook_ReturnsNoContent()
        {
            var bookId = 1;
            var book = new UpdateBookRequest { Title = "Updated Book", Author = "Updated Author" };

            _mockBookService.Setup(service => service.UpdateBookAsync(bookId, It.IsAny<UpdateBookRequest>()))
                .ReturnsAsync(new Book { Id = bookId, Title = "Updated Book", Author = "Updated Author" });

            var result = await _controller.UpdateBook(bookId, book);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateBook_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            var bookId = 1;
            UpdateBookRequest updateRequest = null;

            // Act
            var result = await _controller.UpdateBook(bookId, updateRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateBook_WithInvalidId_ThrowsKeyNotFoundException_ReturnsNotFound()
        {

            var bookId = 999;  
            var updateRequest = new UpdateBookRequest { Title = "Updated Book", Author = "Updated Author" };

            _mockBookService.Setup(service => service.UpdateBookAsync(bookId, updateRequest))
                .ThrowsAsync(new KeyNotFoundException($"Book with ID {bookId} not found"));


            var result = await _controller.UpdateBook(bookId, updateRequest);


            Assert.IsType<NotFoundResult>(result);
        }



        [Fact]
        public async Task DeleteBook_WithValidId_ReturnsNoContent()
        {
            var bookId = 1;

            _mockBookService.Setup(service => service.DeleteBookAsync(bookId))
                .ReturnsAsync(true);

            var result = await _controller.DeleteBook(bookId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_WithInvalidId_ReturnsNotFound()
        {
            var bookId = 999;
            _mockBookService.Setup(service => service.DeleteBookAsync(bookId))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteBook(bookId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
