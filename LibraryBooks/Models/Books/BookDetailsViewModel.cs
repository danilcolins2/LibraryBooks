using LibraryBooks.Models.DbModels;
using System.Security.Policy;
namespace LibraryBooks.Models.Books
{
    public class BookDetailsViewModel 
    {
        public required Book Book { get; set; }
    }
}
