using Database;
using Database.DbModels;
using Database.User;
using Microsoft.AspNetCore.Mvc;
using FilmHub.Services.User;
using FilmHub.Controllers;
using FilmHub.Services.Registration;

namespace FilmHub.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult ShowPersonalPage(string name)
        {
            if (!IRegistrationService.isLogged)
            {
                return RedirectToAction("Index", "Registration");
            }

            User user = new User();
            user = _userService.FindById(IRegistrationService.currentUserId);
            return View(user);
        }
        
        [HttpPost]
        public IActionResult ShowPersonalPage()
        {
            IRegistrationService.isLogged = false;
            return RedirectToAction("Index", "Registration");
        }

        [HttpGet]
        public IActionResult AddToFavourite(string currentFilmImage)
        {
            int currentUserId = IRegistrationService.currentUserId;
            _userService.AddToFavourite(currentFilmImage, currentUserId);
            return RedirectToAction("FilmInfo", "Film");
        }
    }
}