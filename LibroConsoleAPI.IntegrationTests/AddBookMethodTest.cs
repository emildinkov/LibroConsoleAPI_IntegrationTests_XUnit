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
    public class AddBookMethodTest : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public AddBookMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task AddBookAsync_ShouldAddBook()
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

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal("Some title", newBook.Title);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMaxLengthTitleShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = new string('A', 255),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal(255, newBook.Title.Length);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassEmptyTitle_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);

        }

        [Fact]
        public async Task AddBookAsync_WhenPassBiggerLengthTitle_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = new string('A', 256),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassSingleLetterTitle_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "A",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal("A", newBook.Title);
            Assert.Equal("John Doe", newBook.Author);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassSingleLetterAuthor_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "J",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal("J", newBook.Author);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMaxLengthAuthor_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = new string('A', 100),
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal(100, newBook.Author.Length);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassBiggerLengthAuthor_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = new string('A', 101),
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassEmptyAuthor_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassSmallerFromMaxLengthISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Camble",
                ISBN = new string('1', 12),
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassBiggerMaxLengthISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "12345678901233",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);

        }

        [Fact]
        public async Task AddBookAsync_WhenPassEmptyISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joe Balck",
                ISBN = "",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMinOnTheBorderYearPublished_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 1700,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMaxOnTheBorderYearPublished_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassUnderMinOnTheBorderYearPublished_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 1699,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassOverMaxOnTheBorderYearPublished_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2025,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMaxLengthGenre_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = new string('A', 50),
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassOverMaxLengthGenre_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2025,
                Genre = new string('A', 51),
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassEmptyGenre_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2025,
                Genre = "",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMinPages_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = "Some",
                Pages = 1,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
        }
        [Fact]
        public async Task AddBookAsync_WhenPassZeroPages_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = "Some",
                Pages = 0,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task AddBookAsync_WhenPassMinPrice_ShouldAddTheBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = "Some",
                Pages = 100,
                Price = 0.01
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
        }
        [Fact]
        public async Task AddBookAsync_WhenPassZeroPrice_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some title",
                Author = "Joy Balck",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = "Some",
                Pages = 100,
                Price = 0
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }
    }
}
