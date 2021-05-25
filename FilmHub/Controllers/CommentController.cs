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
using FilmHub.Services.Comment;
using FilmHub.Services.Registration;
using FilmHub.Services.User;
using FilmHub.Services.Film;

namespace FilmHub.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult ToLikeAComment(int commentId)
        {
            _commentService.ToLikeAComment(commentId);
            return RedirectToAction("FilmInfo", "Film");
        }
        
    }
}