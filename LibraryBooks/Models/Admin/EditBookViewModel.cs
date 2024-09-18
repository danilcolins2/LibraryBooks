using LibraryBooks.Models.DbModels;
namespace LibraryBooks.Models.Admin
{
    public class EditBookViewModel
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? Genre { get; set; }
        public required string? Description { get; set; }
        public string? ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
        public IFormFile? FormFile { get; set; }

        public Book? Book { get; set; }
    }
}
