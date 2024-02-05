using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class SearchByTitleMethodTest : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public SearchByTitleMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task SearchByTitleAsync_WhenPassFragmentTitle_ShouldReturnBook()
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
            await _bookManager.SearchByTitleAsync("some");

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("Some title", bookInDb.Title);
        }

        [Fact]
        public async Task SearchByTitleAsync_WhenPassWrongTitleFragmant_ShouldReturnBook()
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
            var exception = Assert.ThrowsAsync<KeyNotFoundException> ( () => _bookManager.SearchByTitleAsync("123"));

            // Assert
            Assert.Equal("No books found with the given title fragment.", exception.Result.Message);
        }

        [Fact]
        public async Task SearchByTitleAsync_WhenPassEmptyTitleFragmant_ShouldReturnBook()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.SearchByTitleAsync(""));

            // Assert
            Assert.Equal("Title fragment cannot be empty.", exception.Result.Message);
        }
    }
}
