using Microsoft.AspNetCore.Mvc;
using LibraryBooks.Repositories;
using Microsoft.AspNetCore.Identity;
using LibraryBooks.Models.Account;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using LibraryBooks.Consts;
using LibraryBooks.Models.DbModels;
using LibraryBooks.Models.Shared;
using System.Text.Json;

namespace LibraryBooks.Controllers
{
    public class AccountController : Controller { 

        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository) => _userRepository = userRepository;

    
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User? user = _userRepository.GetLibraryContext().Users.FirstOrDefault(u => u.Login == loginModel.Login && u.Password == loginModel.Password);
                if (user != null)
                {
                    string userLogin = user.Login;
                    string userRole = user.Role;
                    Authenticate(userLogin, userRole);

                    ToastViewModel toastViewModel = new ToastViewModel();

                    if (user.Role == Role.Admin)
                        toastViewModel.AddToast(Strings.AdminHelloMessage, BootstrapColor.Primary);

                    toastViewModel.AddToast(Strings.LoginSuccessfully, BootstrapColor.Success);

                    TempData["ToastModel"] = toastViewModel.AsJson();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ToastModel"] = new ToastViewModel(Strings.IncorrectInputData, BootstrapColor.Danger).AsJson();
                }
            } else {
                
            }
            TempData["ToastModel"] = new ToastViewModel(Strings.UnexpectedError, BootstrapColor.Danger).AsJson();
            return View(loginModel);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerModel)
        {
            if(ModelState.IsValid)
            {
                var users = _userRepository.GetAllUsers();

                bool loginValid = !users.Any(u => u.Login == registerModel.Login);
                bool emailValid = !users.Any(u => u.Email == registerModel.Email);

                ToastViewModel toastViewModel = new ToastViewModel();
                if(loginValid == false)
                {
                    toastViewModel.AddToast(Strings.LoginBusyError, BootstrapColor.Danger);
                } 
                if(emailValid == false)
                {
                    toastViewModel.AddToast(Strings.EmailBusyError, BootstrapColor.Danger);
                }
                if(loginValid == true && emailValid == true) 
                {
                    var newUser = new Models.DbModels.User
                    {
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        Email = registerModel.Email,
                        Login = registerModel.Login,
                        Password = registerModel.Password,
                        Role = Codes.SpecialCodes.Contains(registerModel.SpeciaСode ?? "") ? Role.Admin : Role.User
                    };

                    _userRepository.AddUser(newUser);

                    Authenticate(newUser.Login, newUser.Role);

                    if(newUser.Role == Role.Admin)
                        toastViewModel.AddToast(Strings.AdminHelloMessage, BootstrapColor.Primary);

                    toastViewModel.AddToast(Strings.RegisterSuccessfully, BootstrapColor.Success);
                    TempData["ToastModel"] = toastViewModel.AsJson();
                    return RedirectToAction("Index", "Home");
                }
                TempData["ToastModel"] = toastViewModel.AsJson();
                return View(registerModel);

            }
            TempData["ToastModel"] = new ToastViewModel(Strings.UnexpectedError, BootstrapColor.Danger).AsJson();
            return View();

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private async void Authenticate(string userLogin, string userRole)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userLogin),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole),
            };  

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ToastModel"] = new ToastViewModel(Strings.LoggedOutProfile, BootstrapColor.Warning, BootstrapColor.Dark).AsJson();
            return RedirectToAction("Login", "Account");
        }

        //public IActionResult Toast(ToastViewModel toastModel)
        //{
        //    return PartialView(toastModel);
        //}
    }
}
