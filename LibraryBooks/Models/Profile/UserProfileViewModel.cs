

using System.ComponentModel.DataAnnotations;
using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Models.Profile
{

    public class UserProfileViewModel
    {

        public required User User { get; set; }

        public IEnumerable<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}
