using LibraryBooks.Models.Books;
using LibraryBooks.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace LibraryBooks.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult Index(int? page, string? searchQuery)
        {

            var books = from b in _bookRepository.GetAllBooks()
                        where b.Title.Contains(searchQuery ?? "") || b.Author.Contains(searchQuery ?? "")
                        select b;
            return View(new BooksViewModel(searchQuery, books, page.HasValue ? page.Value : 1, 3));
        }

        public IActionResult Details(int bookId, string backHref)
        {
            var book = _bookRepository.GetBookById(bookId);
            if(book != null)
            {
                var bookModel = new BookDetailsViewModel
                {
                    Book = book,
                };
                return View(bookModel);
            } 
            return View();
        }
    }
}
