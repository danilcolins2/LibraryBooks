using System.ComponentModel.DataAnnotations;

namespace LibraryBooks.Models.DbModels
{
    public class ImageFile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public void Update(ImageFile fileModel)
        {
            Name = fileModel.Name;
            Path = fileModel.Path;
        }
    }
}
