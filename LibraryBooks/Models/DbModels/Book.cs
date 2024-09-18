using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBooks.Models.DbModels
{
    public class Book
    {
        [Key] // Обозначает первичный ключ
        public int Id { get; set; }

        [Required] // Обязательное поле
        [MaxLength(255)] // Максимальная длина поля 255 символов
        public required string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Author { get; set; }

        public string? Genre { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? ISBN { get; set; }

        public DateTime? PublicationDate { get; set; }

        public int NumberBorrows { get; set; } = 0;

        public int? ImageFileId { get; set; }

        // Навигационные свойства для отношений с другими моделями
        [ForeignKey("ImageFileId")]
        public ImageFile? ImageFile { get; set; }

        // Навигационное свойство для отношения "один-ко-многим" с моделью Borrow
        public List<Borrow> Borrows { get; set; } = new();

        public string GetImagePathOrDefault() => ImageFile?.Path ?? Consts.FileConsts.NullBookCoverPath;

        public void Update(Book book)
        {
            Title = book.Title;
            Author = book.Author;
            Genre = book.Genre;
            Description = book.Description;
            ISBN = book.ISBN;
            PublicationDate = book.PublicationDate;
            NumberBorrows = book.NumberBorrows;
            ImageFileId = book.ImageFileId;
        }
    }
}
