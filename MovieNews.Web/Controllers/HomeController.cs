using MovieNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MovieNews.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var movies = await Movies.GetLatestMovies();
            return View(movies);
        }

        public async Task<ActionResult> Details(string title)
        {
            var details = await Movies.GetMovieInfo(title);
            if (details.HasMovie)
            {
                return View(details.Movie);
            }
            else
            {
                return View("NotFound");
            }
        }
    }
}