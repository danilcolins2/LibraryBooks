using LibraryBooks.Models.DbModels;
using Microsoft.EntityFrameworkCore;
namespace LibraryBooks.Repositories
{

    public interface IBookRepository : ILibraryContext
    {
        public IEnumerable<Book> GetAllBooks();
        public Book? GetBookById(int id);
        public void AddBook(Book book);
        public void UpdateBook(int id, Book book);
        public void UpdateNumberOfBorrows(int id, int numberBorrows);
        public bool DeleteBook(int id);

    }

    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;
        public BookRepository(LibraryContext context) => _context = context;

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.Include(b => b.ImageFile);
        }

        public Book? GetBookById(int id)
        {
            return _context.Books.Include(b => b.ImageFile).FirstOrDefault(b => b.Id == id);
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(int id, Book book)
        {
            var existingBook = GetBookById(id);
            if (existingBook != null)
            {
                existingBook.Update(book);
                _context.SaveChanges();
            }
        }

        public bool DeleteBook(int id)
        {
            var existingBook = GetBookById(id);
            if (existingBook != null)
            {
                _context.Books.Remove(existingBook);
                _context.SaveChanges();
                return true;
            }
            return false;

        }

        public LibraryContext GetLibraryContext()
        {
            return _context;
        }

        public void UpdateNumberOfBorrows(int id, int numberBorrows)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                book.NumberBorrows += numberBorrows;

                _context.SaveChanges();
            }
        }
    }
    
}
