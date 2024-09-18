using LibraryBooks.Consts;
using LibraryBooks.Models.Admin;
using LibraryBooks.Models.DbModels;
using LibraryBooks.Models.Profile;
using LibraryBooks.Repositories;
using LibraryBooks.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryBooks.Models.Shared;

namespace LibraryBooks.Controllers
{

    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IImageRepository _imageFileRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        public AdminController(IBookRepository bookRepository, IUserRepository userRepository, IBorrowRepository borrowRepository,IImageRepository imageRepository, IWebHostEnvironment appEnvironment)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _borrowRepository = borrowRepository;
            _imageFileRepository = imageRepository;
            _appEnvironment = appEnvironment;
        }

        public IActionResult AddBook()
        {
            return View();
        }

        
        
        [HttpPost]
        public async Task<IActionResult> AddBook(AddedBookModel addedBookModel)
        {
            ImageFile? imageFile = null;
            if(addedBookModel.FormFile != null)
            {
                string ext = Path.GetExtension(addedBookModel.FormFile.FileName);
                string fileName = $@"{Guid.NewGuid()}.{ext}";

                imageFile = await FileHelper.AddImageToFiles(_appEnvironment.WebRootPath, FileConsts.BookCoversPath, fileName,  addedBookModel.FormFile);
                
                _imageFileRepository.AddFile(imageFile);
            } 
            var newBook = new Book
            {
                Title = addedBookModel.Title,
                Author = addedBookModel.Author,
                Genre = addedBookModel.Genre,
                Description = addedBookModel.Description,
                ISBN = addedBookModel.ISBN,
                PublicationDate = addedBookModel.PublicationDate,
                ImageFile = imageFile
            };

            _bookRepository.AddBook(newBook);

            TempData["ToastModel"] = new ToastViewModel($"Книга \"{newBook.Title}\" добавлена", BootstrapColor.Success).AsJson();
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book != null)
            {
                var bookTitle = book.Title;
                //Удаляем файл обложки, если он есть
                if (book.ImageFileId.HasValue)
                {
                    var lastImage = _imageFileRepository.GetFileById(book.ImageFileId.Value);
                    await FileHelper.DeleteImageFromFile(_appEnvironment.WebRootPath, lastImage);

                    if (lastImage != null)
                        _imageFileRepository.DeleteFile(lastImage.Id);
                }
                if (_bookRepository.DeleteBook(bookId))
                {
                    //Если удалили успешно
                    TempData["ToastModel"] = new ToastViewModel($"Книга \"{bookTitle}\" удалена", BootstrapColor.Warning, BootstrapColor.Dark).AsJson();

                }
            }
            return RedirectToAction("Index", "Books");
        }

        public IActionResult EditBook(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book != null)
            {
                var model = new EditBookViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    Description = book.Description,
                    ISBN = book.ISBN,
                    PublicationDate = book.PublicationDate,
                    Book = book
                };
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(EditBookViewModel editBookModel)
        {
            Console.WriteLine("Вызван метод");

            var editBook = _bookRepository.GetBookById(editBookModel.Id);
            if(editBook != null)
            {
                Console.WriteLine("editBook не нулл");

                var formFile = editBookModel.FormFile;
                ImageFile? imageFile = null;
                if(formFile != null)
                {
                    //Сохраняю файл и добавляем строку в бд
                    string ext = Path.GetExtension(formFile.FileName);
                    string fileName = $@"{Guid.NewGuid()}.{ext}";

                    imageFile = await FileHelper.AddImageToFiles(_appEnvironment.WebRootPath, FileConsts.BookCoversPath, fileName, formFile);
                    _imageFileRepository.AddFile(imageFile);

                    //Удаляем прошлый файл, если он есть
                    if (editBook.ImageFileId.HasValue)
                    {
                        var lastImage = _imageFileRepository.GetFileById(editBook.ImageFileId.Value);
                        await FileHelper.DeleteImageFromFile(_appEnvironment.WebRootPath, lastImage);

                        if (lastImage != null)
                            _imageFileRepository.DeleteFile(lastImage.Id);
                    }
                }

                _bookRepository.UpdateBook(editBook.Id, new Book
                {
                    Title = editBookModel.Title,
                    Author = editBookModel.Author,
                    Genre = editBookModel.Genre,
                    Description = editBookModel.Description,
                    ISBN = editBookModel.ISBN,
                    PublicationDate = editBookModel.PublicationDate,
                    NumberBorrows = editBook.NumberBorrows,
                    ImageFileId = imageFile?.Id ?? editBook.ImageFile?.Id,

                });

                TempData["ToastModel"] = new ToastViewModel(Strings.DataUpdatedSuccessfully, BootstrapColor.Success).AsJson();
                return RedirectToAction("Details", "Books", new { bookId = editBook.Id});
            }
            TempData["ToastModel"] = new ToastViewModel(Strings.UnexpectedError, BootstrapColor.Danger).AsJson();
            return View();
        }

        public IActionResult Borrows()
        {
            var borrows = _borrowRepository.GetAllBorrows();
            return View(borrows);
        }
        public IActionResult UserBorrows(int UserId)
        {
            var user = _userRepository.GetUserById(UserId);
            if (user != null)
            {
                var userBorrows = from b in _borrowRepository.GetAllBorrows()
                                  where b.UserId == UserId
                                  orderby b.ReturnDate.HasValue, b.BorrowDate
                                  select b;

                return View(new UserBorrowsViewModel { User = user, UserBorrows = userBorrows });
            }
            return View();
        }

        [HttpGet]
        public IActionResult MarkReturned(int UserId, int BorrowId)
        {
            _borrowRepository.SetReturnedDate(BorrowId, DateTime.Now);
            return RedirectToAction("UserBorrows", new {UserId = UserId});
        }
        [HttpGet]
        public IActionResult MarkNotReturned(int UserId, int BorrowId)
        {
            _borrowRepository.SetReturnedDate(BorrowId, null);
            return RedirectToAction("UserBorrows", new { UserId = UserId });
        }

        public IActionResult Users()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult BorrowBook(int userId)
        {
            // Получение пользователя
            var user = _userRepository.GetUserById(userId);

            // Получение списка книг
            var books = _bookRepository.GetAllBooks();

            // Создание модели представления
            var viewModel = new BorrowBookViewModel
            {
                UserId = userId,
                Books = books,  // Передача списка книг в модель
                SelectedBookId = 0 // Изначально никакая книга не выбрана
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult BorrowBook(int UserId, int SelectedBookId)
        {
            User? user = _userRepository.GetUserById(UserId);
            Book? book = _bookRepository.GetBookById(SelectedBookId);

            if(book != null && user != null)
            {
                var borrow = new Borrow
                {
                    BorrowDate = DateTime.Now,
                    ReturnDate = null, // Изначально книга не возвращена
                    Book = book,
                    User = user
                };

                _borrowRepository.AddBorrow(borrow);

                _bookRepository.UpdateNumberOfBorrows(SelectedBookId, 1);

                // Перенаправление на страницу со списком пользователей
                TempData["ToastModel"] = new ToastViewModel(Strings.BorrowBookSuccessfully, BootstrapColor.Success).AsJson();
                return RedirectToAction("Users");
            }

            TempData["ToastModel"] = new ToastViewModel(Strings.UnexpectedError, BootstrapColor.Danger).AsJson();
            return RedirectToAction("BorrowBook", new { userId = UserId });
        }


    }

}

