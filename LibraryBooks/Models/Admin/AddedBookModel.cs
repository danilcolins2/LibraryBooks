using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Models.Admin
{
    public class AddedBookModel
    {
        public required string Title { get; set; }
        public required string Author { get; set; }

        public string? Genre { get; set; }
        public required string? Description { get; set; }
        public required string ISBN { get; set; }
        
        public DateTime? PublicationDate { get; set; }

        public IFormFile? FormFile { get; set; }

    }
}
