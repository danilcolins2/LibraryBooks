using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBooks.Models.DbModels
{


    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Login { get; set; }

        [Required]
        [MaxLength(255)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }

        [Required]
        [MaxLength(25)]
        public required string Role { get; set; }

        public int? ImageFileId { get; set; }

        // Навигационные свойства для отношений с другими моделями
        [ForeignKey("ImageFileId")]
        public ImageFile? ImageFile { get; set; }

        public string GetImagePathOrDefault() => ImageFile?.Path ?? Consts.FileConsts.NullUserAvatarPath;
        public void Update(User user)
        {
            Login = user.Login;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Password = user.Password;
            Role = user.Role;
            ImageFileId = user.ImageFileId;
        }

        // Навигационное свойство для отношения "один-ко-многим" с моделью Borrow
        //public List<Borrow> Borrows { get; set; } = new();


    }
}
