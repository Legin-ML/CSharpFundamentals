﻿using Azure.Core;
using Microservice.Data.Repositories;
using Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Services
{
    public class BookService : IBookService
    {

        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger) 
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        public async Task<Book> AddBookAsync(CreateBookRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var book = new Book
            {
                Author = request.Author,
                Genre = request.Genre,
                Price = request.Price,
                PublishedDate = request.PublishedDate,
                Title = request.Title
            };

            return await _bookRepository.AddBookAsync(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var ifExists = await _bookRepository.BookExistsAsync(id);
            
            if(!ifExists)
            {
                _logger.LogWarning($"Book with ID {id} not found for deletion");
                throw new KeyNotFoundException($"Book with ID {id} not found");
            }

            return await _bookRepository.DeleteBookAsync(id);

        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found");
                throw new KeyNotFoundException($"Book with ID {id} not found");
            }
            return book;
        }

        public async Task<Book> UpdateBookAsync(int id, UpdateBookRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));


            var ifExists = await _bookRepository.BookExistsAsync(id);
            if (!ifExists)
            {
                _logger.LogWarning($"Book with ID {id} not found for update");
                throw new KeyNotFoundException($"Book with ID {id} not found");
            }

            var book = new Book
            {
                Id = id,
                Author = request.Author,
                Genre = request.Genre,
                Price = request.Price,
                PublishedDate = request.PublishedDate,
                Title = request.Title
            };


            return await _bookRepository.UpdateBookAsync(book);
        }
    }
}
