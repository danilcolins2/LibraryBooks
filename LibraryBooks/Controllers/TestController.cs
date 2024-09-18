using LibraryBooks.Models.DbModels;
using LibraryBooks.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooks.Controllers
{
    public class TestController : Controller
    {

        IImageRepository _fileRepository;
        IWebHostEnvironment _appEnvironment;
        public TestController(IImageRepository fileRepository, IWebHostEnvironment appEnvironment)
        {
            _fileRepository = fileRepository;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(_fileRepository.GetAllFiles());
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!    " + System.IO.Path.GetExtension(uploadedFile.FileName));
                Console.WriteLine("!!!!!!!!!!!!!!!!!    " + System.IO.Path.GetFileName(uploadedFile.FileName));
                // путь к папке Files
                string path = "/Files/" + "UserAva1" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                ImageFile file = new ImageFile { Name = uploadedFile.FileName, Path = path };
                _fileRepository.AddFile(file);

            }

            return RedirectToAction("Index");
        }
    }
}
