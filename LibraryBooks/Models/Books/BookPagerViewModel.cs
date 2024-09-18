using LibraryBooks.Models.DbModels;
namespace LibraryBooks.Models.Books
{
    public class BooksViewModel
    {
        public string? SearchQuery { get; set; }
        public IEnumerable<Book> Books { get; set; }

        private int _page;
        public int Page
        {
            get { return _page; } 
            set { _page = (value > 0) ? value : 1; }
        }
        public int CountBooksOnPage { get; set; } 

        public int CountPages { get; set; }

        public BooksViewModel(string? searchQuery, IEnumerable<Book> books, int page, int countBooksOnPage)
        {
            SearchQuery = searchQuery;
            Page = page;
            CountBooksOnPage = countBooksOnPage;

            CountPages = (int)Math.Ceiling((double)books.Count() / (double)CountBooksOnPage);
            Books = books.Skip((Page - 1) * countBooksOnPage).Take(countBooksOnPage);
        }        
    }
}
