using LibraryBooks.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace LibraryBooks.Models.Profile
{
    public class EditUserProfileViewModel
    {
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public IFormFile? FormFile { get; set; }
        public User? User { get; set; }
    }
}
