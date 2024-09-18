using LibraryBooks.Models.DbModels;

namespace LibraryBooks.Data
{
    public class FileHelper
    {
        public static async Task<ImageFile> AddImageToFiles(string webRootPath, string directoryPath, string fileName, IFormFile formFile)
        {
            var dbFilePath = directoryPath + fileName;
            using (var fileStream = new FileStream(webRootPath + dbFilePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return new ImageFile { Name = fileName, Path = dbFilePath };
        }

        public static async Task DeleteImageFromFile(string webRootPath, ImageFile? imageFile)
        {
            if (imageFile == null)
            {
                return; // Ничего не удалять, если ImageFile пуст
            }

            var filePath = webRootPath + imageFile.Path;

            if (File.Exists(filePath))
            {
                Console.WriteLine("!!!!!!!!!!!!!!! Удалениеее");
                Console.WriteLine("!!!!!!!!!!!!!!! Путь " + filePath);
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
