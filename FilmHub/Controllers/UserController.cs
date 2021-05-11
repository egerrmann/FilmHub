using System.Collections.Generic;
using Database;
using Database.DbModels;
using Database.Film;
using Database.User;
using Microsoft.AspNetCore.Mvc;
using FilmHub.Services.User;
using FilmHub.Controllers;
using FilmHub.Models;
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
            ViewBag.RecommendedFilmsDirector = _userService.RecommendedFilmsDirector(IRegistrationService.currentUserId);
            ViewBag.RecommendedFilmsGenre = _userService.RecommendedFilmsGenre(IRegistrationService.currentUserId);
            return View(user);
        }

        [HttpGet]
        public IActionResult ChangeUserPassword()
        {
            var currentUser = _userService.FindById(IRegistrationService.currentUserId);
            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                Name = currentUser.Name,
                Password = currentUser.Password,
                Favourite = currentUser.Favourite
            };
            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult ChangeUserPassword(UserViewModel model)
        {
            _userService.ChangeUserPassword(IRegistrationService.currentUserId, model.Password);
            return RedirectToAction("ShowPersonalPage", "User");
        }
        
        [HttpPost]
        public IActionResult ShowPersonalPage()
        {
            IRegistrationService.isLogged = false;
            IRegistrationService.currentUserId = 0;
            return RedirectToAction("Index", "Registration");
        }

        [HttpGet]
        public IActionResult AddToFavourite(string currentFilmImage)
        {
            int currentUserId = IRegistrationService.currentUserId;
            if (currentUserId == 0)
            {
                return RedirectToAction("LogIn", "Registration");
            }
            _userService.AddToFavourite(currentFilmImage, currentUserId);
            return RedirectToAction("AllFilms", "Film");
        }
    }
}