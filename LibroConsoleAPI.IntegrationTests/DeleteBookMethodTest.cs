using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class DeleteBookMethodTest : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public DeleteBookMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldDeleteBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task DeleteBookAsync_WhenPassMoreBooks_ShouldDeleteOneBook()
        {
            // Arrange
            var firstBook = new Book
            {
                Title = "Some title one",
                Author = "John Doese",
                ISBN = "1234567890444",
                YearPublished = 2022,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };
            var secondBook = new Book
            {
                Title = "Some title two",
                Author = "John Doebb",
                ISBN = "1234567890333",
                YearPublished = 2020,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };
            var thirdBook = new Book
            {
                Title = "Some title three",
                Author = "John Doerh",
                ISBN = "1234567890222",
                YearPublished = 2023,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(firstBook);
            await _bookManager.AddAsync(secondBook);
            await _bookManager.AddAsync(thirdBook);

            // Act
            await _bookManager.DeleteAsync(firstBook.ISBN);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("Some title two", secondBook.Title);
            Assert.Equal("Some title three", thirdBook.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_WhenPassEmptyISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.DeleteAsync(""));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_WhenPassWrongISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => _bookManager.DeleteAsync("123456"));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'entity')", exception.Result.Message);
        }
    }
}
