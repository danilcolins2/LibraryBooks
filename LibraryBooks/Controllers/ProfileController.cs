using LibraryBooks.Consts;
using LibraryBooks.Data;
using LibraryBooks.Models.Admin;
using LibraryBooks.Models.DbModels;
using LibraryBooks.Models.Profile;
using LibraryBooks.Models.Shared;
using LibraryBooks.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LibraryBooks.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {

        private IUserRepository _userRepository;
        private IBorrowRepository _borrowRepository;
        private readonly IImageRepository _imageFileRepository;
        private readonly IWebHostEnvironment _appEnvironment;

        public ProfileController(IUserRepository userRepository, IBorrowRepository borrowRepository, IImageRepository imageRepository, IWebHostEnvironment appEnvironment)
        {
            _borrowRepository = borrowRepository;
            _userRepository = userRepository;
            _imageFileRepository = imageRepository;
            _appEnvironment = appEnvironment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            string? login = User?.Identity?.Name;
            if (login != null)
            {
                User? user = _userRepository.GetUserByLogin(login);
                if (user != null)
                {
                    var userBorrows = from b in _borrowRepository.GetAllBorrows()
                                      where b.UserId == user.Id
                                      orderby b.BorrowDate descending
                                      select b;

                    var lastThreeBorrows = userBorrows.Take(3);

                    UserProfileViewModel userProfileViewModel = new UserProfileViewModel
                    {
                        User = user,
                        Borrows = lastThreeBorrows,
                    };
                    return View(userProfileViewModel);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult UserBorrows()
        {
            string? login = User?.Identity?.Name;
            if (login != null)
            {
                User? user = _userRepository.GetUserByLogin(login);
                if (user != null)
                {
                    var userBorrows = from b in _borrowRepository.GetAllBorrows()
                                      where b.UserId == user.Id
                                      orderby b.ReturnDate.HasValue, b.BorrowDate
                                      select b;

                    return View(new UserBorrowsViewModel { User = user, UserBorrows = userBorrows });
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit()
        {
            string? login = User?.Identity?.Name;
            if (login != null)
            {
                User? user = _userRepository.GetUserByLogin(login);
                if (user != null)
                {

                    EditUserProfileViewModel editUserProfileViewModel = new EditUserProfileViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,
                        User = user,
                    };
                    return View(editUserProfileViewModel);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserProfileViewModel editUserProfileViewModel)
        {
            if(ModelState.IsValid)
            {
                string? login = User?.Identity?.Name;
                if (login != null)
                {
                    var user = _userRepository.GetUserByLogin(login);
                    if (user != null)
                    {

                        var formFile = editUserProfileViewModel.FormFile;
                        ImageFile? imageFile = null;
                        if (formFile != null)
                        {
                            string ext = Path.GetExtension(formFile.FileName);
                            string fileName = $@"{Guid.NewGuid()}.{ext}";

                            imageFile = await FileHelper.AddImageToFiles(_appEnvironment.WebRootPath, FileConsts.UserAvatarsPath, fileName, formFile);

                            _imageFileRepository.AddFile(imageFile);
                            // _userRepository.UpdateImage(user.Id, imageFile);


                            //Удаляем прошлый файл, если он есть
                            if (user.ImageFileId.HasValue)
                            {
                                var lastImage = _imageFileRepository.GetFileById(user.ImageFileId.Value);
                                await FileHelper.DeleteImageFromFile(_appEnvironment.WebRootPath, lastImage);

                                if (lastImage != null)
                                    _imageFileRepository.DeleteFile(lastImage.Id);
                            }
                        } 

                        _userRepository.UpdateUser(
                            user.Id,
                            new User
                            {
                                Login = user.Login,
                                FirstName = editUserProfileViewModel.FirstName,
                                LastName = editUserProfileViewModel.LastName,
                                Email = editUserProfileViewModel.Email,
                                Password = editUserProfileViewModel.Password,
                                Role = user.Role,
                                ImageFileId = imageFile?.Id ?? user.ImageFile?.Id,
                            }
                        );
                        TempData["ToastModel"] = new ToastViewModel(Strings.DataUpdatedSuccessfully, BootstrapColor.Success).AsJson();
                        return RedirectToAction("Index");
                    }
                        
                }
            }
            TempData["ToastModel"] = new ToastViewModel(Strings.UnexpectedError, BootstrapColor.Danger).AsJson();

            return View(editUserProfileViewModel);
        }
    }
}
