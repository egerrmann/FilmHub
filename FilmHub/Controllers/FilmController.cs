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
            //int currentFilmId = IFilmService.currentFilmId;
            Film currentFilm = _filmService.GetCurrentFilmInfo(IFilmService.currentFilmId);
            ViewBag.lastElement = currentFilm.Actors.Last();
            return View(currentFilm);
        }
    }
}