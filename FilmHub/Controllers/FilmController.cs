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

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }
        
        [HttpGet]
        public IActionResult AllFilms()
        {
            List<Film> films = _filmService.GetAllFilms();
            ViewBag.categoriesList = _filmService.GetAllCategories();
            ViewBag.sortedByYearlist = _filmService.SortFilmsByYear();
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
            Film currentFilm = _filmService.GetCurrentFilmInfo(IFilmService.currentFilmId);
            ViewBag.lastElement = currentFilm.Actors.Last();
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
                    Image = film.Image
                };
                foundFilms.Add(currentFilm);
            }
            
            return View(foundFilms);
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

        /*[HttpGet]
        public IActionResult AddToBookmarks(string filmImage)
        {
            if (IRegistrationService.currentUserId == 0)
            {
                return RedirectToAction("LogIn", "Registration");
            }

            var currentFilmId = _filmService.GetCurrentFilmId(filmImage);
            _filmService.AddToBookmarks(currentFilmId, IRegistrationService.currentUserId);
            return RedirectToAction("FilmInfo", "Film");
        }*/
    }
}