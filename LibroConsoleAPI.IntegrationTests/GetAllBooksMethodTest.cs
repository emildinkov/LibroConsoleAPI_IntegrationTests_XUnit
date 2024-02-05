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
    public class GetAllBooksMethodTest : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public GetAllBooksMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task GetAllAsync_WhenPassMoreBooks_ShouldReturnAllBooks()
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
            var listOfBooks = await _bookManager.GetAllAsync();

            // Assert
            Assert.NotNull(listOfBooks);
            Assert.Contains(firstBook, listOfBooks);
            Assert.Contains(secondBook, listOfBooks);
            Assert.Contains(thirdBook, listOfBooks);
        }

        [Fact]
        public async Task GetAllAsync_WhenNotPassBooks_ShouldThrowException()
        {

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetAllAsync());

            // Assert
            Assert.Equal("No books found.", exception.Result.Message);
        }
    }
}
