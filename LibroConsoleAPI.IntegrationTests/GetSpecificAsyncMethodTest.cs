using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class GetSpecificAsyncMethodTest : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public GetSpecificAsyncMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task GetSpecificAsync_WhenPassEmptySpecificISBN_ShouldThrowException()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.GetSpecificAsync(""));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }

        [Fact]
        public async Task GetSpecificAsync_WhenPassNoExistISBN_ShouldThrowException()
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
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetSpecificAsync("1234567890456"));

            // Assert
            Assert.Equal("No book found with ISBN: 1234567890456", exception.Result.Message);
        }
    }
}
