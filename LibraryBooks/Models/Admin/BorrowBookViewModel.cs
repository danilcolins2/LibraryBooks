using System.ComponentModel.DataAnnotations;
using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Models.Admin
{
    public class BorrowBookViewModel
    {
        public int UserId { get; set; }

        public IEnumerable<Book>? Books { get; set; }
        public int SelectedBookId { get; set; }
    }
}
