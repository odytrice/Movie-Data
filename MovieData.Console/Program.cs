using MovieNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace MovieData.Console
{
    class Program
    {
        static async Task Demo()
        {
            var info = await Movies.GetMovieInfo("The Martian");
            if (info.HasMovie)
            {
                WriteLine(
                    $"Name: {info.Movie.Movie.Title}\n" +
                    $"Poster: {info.Movie.Details.Poster}");
            }
        }
        static void Main(string[] args)
        {
            var demo = Demo();
            WriteLine("Downloading...");
            demo.Wait();
        }
    }
}
