using LibraryBooks.Models.Shared;

namespace LibraryBooks.Models.Account
{
    public class RegisterViewModel
    {
        public required string Login { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? SpeciaСode { get; set; }
        public IFormFile? FormFile { get; set; }
    }

}
