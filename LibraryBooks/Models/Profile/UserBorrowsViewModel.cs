using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Models.Profile
{
    public class UserBorrowsViewModel
    {
        public required User User { get; set; }
        public IEnumerable<Borrow>? UserBorrows { get; set; }

        public string GetUserName()
        {
            if (User != null) return User.FirstName + " " + User.LastName;
            else return "Нет пользователя с таким Id";
        }

    }
}
