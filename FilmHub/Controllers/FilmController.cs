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
using FilmHub.Services.Film;

namespace FilmHub.Controllers
{
    public class FilmController : Controller
    {
        
        private readonly IFilmService _filmService;
        private readonly IUserService _userService;

        public FilmController(IFilmService filmService,IUserService userService)
        {
            _filmService = filmService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult AllFilms(string sort)
        {
            List<Film> films = new List<Film>();
            if (sort == null || sort == "default")
            {
                films = _filmService.GetAllFilms();
                ViewBag.sortType = "Filmhub films";
                ViewBag.button = "Sort by";
            }

            if (sort == "title")
            {
                films = _filmService.SortFilmsByTitle();
                ViewBag.sortType = "Filmhub films sorted by title";
                ViewBag.button = "Title";
            }

            if (sort == "year")
            {
                films = _filmService.SortFilmsByYear();
                ViewBag.sortType = "Filmhub films sorted by year";
                ViewBag.button = "Year";
            }

            if (sort == "duration")
            {
                films = _filmService.SortFilmsByDuration();
                ViewBag.sortType = "Filmhub films sorted by duration";
                ViewBag.button = "Duration";
            }
            ViewBag.list = _filmService.GetAllCategories();
            return View(films);
        }
        
        [HttpGet]
        public IActionResult CurrentFilm(string image)
        {
            IFilmService.currentFilmId = _filmService.GetCurrentFilmId(image);
            return RedirectToAction("FilmInfo", "Film");
        }

        [HttpGet]
        public IActionResult CurrentCategoryFilms(string category)
        {
            List<Film> currentCategoryFilms = _filmService.CurrentCategoryFilms(category);
            ViewBag.currentCategory = category;
            ViewBag.list = _filmService.GetAllCategories();
            return View(currentCategoryFilms);
        }
        
        [HttpGet]
        public IActionResult FilmInfo()
        {
            if (IRegistrationService.currentUserId == 0)
            {
                ViewBag.unregisteredUser = "true";
            }
            else
            {
                User currentUser = _filmService.FindById(IRegistrationService.currentUserId);
                int currentUserYears = (currentUser.DateOfBirth[0] - 48) * 1000 +
                                          (currentUser.DateOfBirth[1] - 48) * 100 +
                                          (currentUser.DateOfBirth[2] - 48) * 10 + (currentUser.DateOfBirth[3] - 48); 
                ViewBag.currentUserYearOfBirth = currentUserYears;
            }
            Film currentFilm = _filmService.GetCurrentFilmInfo(IFilmService.currentFilmId);
            ViewBag.currentFilmId = _filmService.GetCurrentFilmId(currentFilm.Image);
            ViewBag.currentUserId = IRegistrationService.currentUserId;
            ViewBag.lastElement = currentFilm.Actors.Last();
            ViewBag.AllUsers = _userService.GetAllUsers();
            return View(currentFilm);
        }

        [HttpGet]
        public IActionResult SearchAndGetFilms(string parameter)
        {
            ViewBag.parameter = parameter;
            List<FilmViewModel> foundFilms = new List<FilmViewModel>();
            List<Film> films = _filmService.SearchAndGetFilms(parameter);
            foreach (var film in films)
            {
                FilmViewModel currentFilm = new FilmViewModel
                {
                    Title = film.Title,
                    Year = film.Year,
                    Genre = film.Genre,
                    Director = film.Director,
                    Summary = film.Summary,
                    Time = film.Time,
                    Age = film.Age,
                    Country = film.Country,
                    Actors = film.Actors,
                    Image = film.Image,
                    Trailer = film.Trailer,
                    Rating = film.Rating
                };
                foundFilms.Add(currentFilm);
            }
            
            return View(foundFilms);
        }

        public class RatingModel
        {
            public string RatingType { get; set; }

            public string RatingValue { get; set; }
        }
        
        [HttpPost]
        public int GetRating([FromBody]RatingModel model)
        {
            return 3;
        }
        
        [HttpGet]
        public IActionResult LeaveComment(string comment)
        {
            if (IRegistrationService.currentUserId == 0)
            {
                return RedirectToAction("LogIn", "Registration");
            }
            _filmService.LeaveComment(comment, IRegistrationService.currentUserId, IFilmService.currentFilmId);
            return RedirectToAction("FilmInfo", "Film");
        }

        [HttpGet]
        public IActionResult AddToBookmarks(string filmImage)
        {
            if (IRegistrationService.currentUserId == 0)
            {
                return RedirectToAction("LogIn", "Registration");
            }

            var currentFilmId = _filmService.GetCurrentFilmId(filmImage);
            _filmService.AddToBookmarks(currentFilmId, IRegistrationService.currentUserId);
            return RedirectToAction("FilmInfo", "Film");
        }
        
        [HttpGet]
        public IActionResult AddRating(string filmImage,int generalImpression, int actorPlay, int scenario, int filming)
        {
            if (IRegistrationService.currentUserId == 0)
            {
                return RedirectToAction("LogIn", "Registration");
            }

            var currentFilmId = _filmService.GetCurrentFilmId(filmImage);
            _filmService.AddRating(currentFilmId, IRegistrationService.currentUserId,generalImpression, actorPlay, scenario,  filming);
            return RedirectToAction("FilmInfo", "Film");
        }
        
        [HttpGet]
        public IActionResult UpdateRating(string filmImage,int generalImpression, int actorPlay, int scenario, int filming)
        {
            if (IRegistrationService.currentUserId == 0)
            {
                return RedirectToAction("LogIn", "Registration");
            }

            var currentFilmId = _filmService.GetCurrentFilmId(filmImage);
            _filmService.UpdateRating(currentFilmId);
            return RedirectToAction("FilmInfo", "Film");
        }
    }
}