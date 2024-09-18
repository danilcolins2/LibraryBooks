using LibraryBooks.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace LibraryBooks.Models.Account
{
    public class LoginViewModel
    {
        public required string Login { get; set; }
        public required string Password { get; set; }

    }
    

}
