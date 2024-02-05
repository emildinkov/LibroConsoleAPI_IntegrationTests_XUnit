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
    public class UpdateAsyncMethodTest : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public UpdateAsyncMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task UpdateAsync_WhenEditTitle_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("Some", updateBook.Title);
        }

        [Fact]
        public async Task UpdateAsync_WhenEditAuthor_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some title",
                Author = "John",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("John", updateBook.Author);
        }

        [Fact]
        public async Task UpdateAsync_WhenEditISBN_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890444",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("1234567890444", updateBook.ISBN);
        }

        [Fact]
        public async Task UpdateAsync_WhenEditYearPublished_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890444",
                YearPublished = 2023,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal(2023, updateBook.YearPublished);
        }

        [Fact]
        public async Task UpdateAsync_WhenEditGenre_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890444",
                YearPublished = 2021,
                Genre = "Comedy",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("Comedy", updateBook.Genre);
        }

        [Fact]
        public async Task UpdateAsync_WhenEditPages_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890444",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 1000,
                Price = 19.99
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal(1000, updateBook.Pages);
        }

        [Fact]
        public async Task UpdateAsync_WhenEditPrice_ShouldReturnUpdateBook()
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
            var updateBook = new Book
            {
                Title = "Some title",
                Author = "John Doe",
                ISBN = "1234567890444",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 50.00
            };

            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.UpdateAsync(updateBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal(50.00, updateBook.Price);
        }
    }
}
