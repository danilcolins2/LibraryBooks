using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBooks.Models.DbModels
{
    public class Borrow
    {
        [Key]
        public int Id { get; set; }


        public int BookId { get; set; }

        public int UserId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        // Навигационные свойства для отношений с другими моделями
        [ForeignKey("BookId")]
        public required Book Book { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
