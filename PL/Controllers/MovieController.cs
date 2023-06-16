using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Popular()
        {
            ML.Movie movie = new ML.Movie();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
                var responseTask = client.GetAsync("movie/popular?api_key=c9617782375bf747c35d7aafecf16f5b&language=es-MX&page=1");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movie.Movies = new List<object>();

                    foreach (var resultItem in resultJSON.results)
                    {
                        ML.Movie movieItem = new ML.Movie();
                        movieItem.id = resultItem.id;
                        movieItem.original_title = resultItem.original_title;
                        movieItem.overview = resultItem.overview;
                        movieItem.release_date = resultItem.release_date;
                        movieItem.backdrop_path = resultItem.backdrop_path;
                        movieItem.vote_average = resultItem.vote_average;
                        movieItem.original_language = resultItem.original_language;
                        movieItem.poster_path = resultItem.poster_path;
                        movie.Movies.Add(movieItem);
                    }
                }
            }
            return View(movie);
        }
    }
}