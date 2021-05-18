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
using System;

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
                return RedirectToAction("LogIn", "Registration");
            }
            int hour = DateTime.Now.Hour;
            if (hour >= 0 && hour < 4 )
            {
                ViewBag.Greeting = "Good night";
            }
            if (hour >= 4 && hour < 12 )
            {
                ViewBag.Greeting = "Good morning";
            }
            if (hour >= 12 && hour < 17 )
            {
                ViewBag.Greeting = "Good afternoon";
            }
            if (hour >= 17 && hour <= 23 )
            {
                ViewBag.Greeting = "Good evening";
            }
            User user = new User();
            user = _userService.FindById(IRegistrationService.currentUserId);
            ViewBag.RecommendedFilmsDirector = _userService.RecommendedFilmsDirector(IRegistrationService.currentUserId);
            ViewBag.RecommendedFilmsGenre = _userService.RecommendedFilmsGenre(IRegistrationService.currentUserId);
            ViewBag.SimilarUsers = _userService.SimilarUsers(IRegistrationService.currentUserId);
            if (_userService.IsExpert(IRegistrationService.currentUserId))
            {
                ViewBag.isExpert = "true"; 
            }
            return View(user);
        }
        
        [HttpPost]
        public IActionResult ShowPersonalPage()
        {
            IRegistrationService.isLogged = false;
            IRegistrationService.currentUserId = 0;
            return RedirectToAction("LogIn", "Registration");
        }

        [HttpGet]
        public IActionResult EditProfile(string currentUserEmail)
        {
            User currentUser = _userService.FindByEmail(currentUserEmail);
            return View(currentUser);
        }

        [HttpPost]
        public IActionResult EditProfile(string firstName, string lastName, string email, string dateOfBirth,
            string country)
        {
            User currentUser = _userService.FindById(IRegistrationService.currentUserId);
            _userService.EditProfile(firstName, lastName, email, dateOfBirth, country,
                currentUser);
            return RedirectToAction("ShowPersonalPage", "User");
        }

        [HttpGet]
        public IActionResult ChangeUserPassword()
        {
            var currentUser = _userService.FindById(IRegistrationService.currentUserId);
            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                FirstName = currentUser.FirstName,
                Password = currentUser.Password,
                LastName = currentUser.LastName,
                Country = currentUser.Country,
                DateOfBirth = currentUser.DateOfBirth,
                Favourite = currentUser.Favourite
            };

            if (IUserService.ErrorMessage != null)
            {
                ViewBag.errorMessage = IUserService.ErrorMessage;   
            }
            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult ChangeUserPassword(string oldPassword, string newPassword, string newPasswordRepeat)
        {
            if (_userService.ChangedPasswordIsCorrect(IRegistrationService.currentUserId,
                oldPassword, newPassword, newPasswordRepeat) == 1)
            {
                _userService.ChangeUserPassword(IRegistrationService.currentUserId, newPassword);
                return RedirectToAction("ShowPersonalPage", "User");    
            }

            if (_userService.ChangedPasswordIsCorrect(IRegistrationService.currentUserId,
                oldPassword, newPassword, newPasswordRepeat) == 2)
            {
                IUserService.ErrorMessage = "An old and new password are the same";
            }
            if (_userService.ChangedPasswordIsCorrect(IRegistrationService.currentUserId,
                oldPassword, newPassword, newPasswordRepeat) == 3)
            {
                IUserService.ErrorMessage = "You didn't repeat your new password";
            }
            if (_userService.ChangedPasswordIsCorrect(IRegistrationService.currentUserId,
                oldPassword, newPassword, newPasswordRepeat) == 4)
            {
                IUserService.ErrorMessage = "The length should be at least 8 symbols";
            }
            if (_userService.ChangedPasswordIsCorrect(IRegistrationService.currentUserId,
                oldPassword, newPassword, newPasswordRepeat) == 5)
            {
                IUserService.ErrorMessage = "New password should contain at least one digit";
            }
            return RedirectToAction("ChangeUserPassword", "User");
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

        [HttpGet]
        public IActionResult ShowAnotherUserPersonalPage(string anotherUserEmail)
        {
            User anotherUser = _userService.FindByEmail(anotherUserEmail);
            UserViewModel anotherUserViewModel = new UserViewModel
            {
                FirstName = anotherUser.FirstName,
                LastName = anotherUser.LastName,
                Favourite = anotherUser.Favourite
            };
            return View(anotherUserViewModel);
        }
    }
}