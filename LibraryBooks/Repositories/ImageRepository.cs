using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Repositories
{
    public interface IImageRepository : ILibraryContext
    {
        IEnumerable<ImageFile> GetAllFiles();
        ImageFile? GetFileById(int fileId);

        void AddFile(ImageFile file);
        void UpdateFile(int fileId, ImageFile file);
        bool DeleteFile(int fileId);



    }
    public class ImageRepository : IImageRepository
    {
        private readonly LibraryContext _context;

        public ImageRepository(LibraryContext context) => _context = context;
        public void AddFile(ImageFile file)
        {
            _context.Files.Add(file);
            _context.SaveChanges();
        }

        public bool DeleteFile(int fileId)
        {
            var file = GetFileById(fileId);
            if (file != null)
            {
                _context.Files.Remove(file);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    

        public IEnumerable<ImageFile> GetAllFiles()
        {
            return _context.Files;
        }

        public ImageFile? GetFileById(int fileId)
        {
            return _context.Files.FirstOrDefault(f => f.Id == fileId);
        }

        public LibraryContext GetLibraryContext()
        {
            return _context;
        }

        public void UpdateFile(int fileId, ImageFile file)
        {
            var existingFile = GetFileById(fileId);
            if (existingFile != null)
            {
                existingFile.Update(file);
                _context.SaveChanges();
            }
        }
    }
}
