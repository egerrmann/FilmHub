using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;
using Database;
using Database.DbModels;
using Database.Film;
using Database.User;
using Microsoft.AspNetCore.Mvc;
using FilmHub.Models;
using FilmHub.Services.Registration;
using FilmHub.Services.User;


namespace FilmHub.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService _registrationService;
        private readonly IUserService _userService;

        public RegistrationController(IRegistrationService registrationService, IUserService userService)
        {
                _registrationService = registrationService;
                _userService = userService;
        }

        [HttpGet]
        public IActionResult Index(string email, string password, string name)
        {
            var model = new UserViewModel()
            {
                Email = email,
                Password = password,
                Name = name,
                Favourite = new List<Film>()
            };
            return View(model);
        }
        

        [HttpPost]
        public IActionResult Index(UserViewModel model)
        {
            var newUser = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Favourite = new List<Film>()
            };
            var userId = _userService.Add(newUser);
            IRegistrationService.isLogged = true;
            IRegistrationService.currentUserId = userId;
            return RedirectToAction("ShowPersonalPage", "User");
        }
        

        [HttpGet]
        public IActionResult LogIn(string name, string password)
        {
            var count = _userService.Count();
            if (count == 0)
            {
                return RedirectToAction("Index", "Registration");
            }

            var logInModel = new UserViewModel()
            {
                Name = name,
                Password = password
            };
            return View(logInModel);
        }

        [HttpPost]
        public IActionResult LogIn(UserViewModel logInModel)
        {
            var currUser = new User()
            {
                Name = logInModel.Name,
                Email = logInModel.Email,
                Password = logInModel.Password
            };
            bool correct = _userService.Find(currUser);
            if (correct == true)
            {
                IRegistrationService.isLogged = true;
                IRegistrationService.currentUserId = _userService.CurrentUser_Id(currUser);
                return RedirectToAction("ShowPersonalPage", "User");
            }
            else
            {
                return RedirectToAction("LogIn", "Registration");
            }
        }
    }
}