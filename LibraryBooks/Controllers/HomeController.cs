using LibraryBooks.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryBooks.Consts;
using LibraryBooks.Models.Shared;

namespace LibraryBooks.Controllers
{

    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public HomeController(IBookRepository bookRepository )
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            var books = (from b in _bookRepository.GetAllBooks() 
                        orderby b.NumberBorrows descending
                        select b).Take(5);
            return View(books);
        }

        public IActionResult About()
        {
            return  View();
        }


        //Частички
        public IActionResult Header()
        {
            return PartialView();
        }

        public IActionResult Footer()
        {
            return PartialView();
        }

        public IActionResult Toast(ToastViewModel toastModel)
        {
            return PartialView(toastModel);
        }
    }
}
