using System.Security.Claims;

namespace LibraryBooks.Models.Header
{
    public class HeaderViewModel
    {
        private static readonly List<HeaderMenuItem>  _menuItems = new List<HeaderMenuItem>
        {
            new("Главная", "Home", "Index"),
            new("Книги", "Books", "Index"),
            new("О нас", "Home", "About"),
            
        };
        private static readonly List<HeaderMenuItem> _adminMenuItems = new List<HeaderMenuItem>
        {
            new("Пользователи", "Admin", "Users"),
            new("Добавить книгу", "Admin", "AddBook"),
        };

        public required ClaimsPrincipal User { get; set; }

        public IEnumerable<HeaderMenuItem> MenuItems { get; set; } = _menuItems;
        public IEnumerable<HeaderMenuItem> AdminMenuItems { get; set; } = _adminMenuItems;

    }
}
