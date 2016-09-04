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

        public ActionResult Details(string title)
        {
            return View();
        }
    }
}