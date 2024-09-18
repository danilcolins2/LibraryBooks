using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Repositories
{
    public interface ILibraryContext
    {
        public LibraryContext GetLibraryContext();
    }
}
