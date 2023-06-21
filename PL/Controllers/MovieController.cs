using ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PL.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Popular()
        {
            ML.Movie movie = new ML.Movie();
            string session_id = Convert.ToString(Session["SessionId"]);
            //Models.User user = (Models.User)Session["user"]; PARA PASAR TODO EL MODELO ENTRE VISTAS
            if (session_id == "")
            {
                return RedirectToAction("Form", "Login");
            }
            else
            {
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

        // GET: Favorite Movies
        public ActionResult Favoritas()
        {
            ML.Movie popular = new ML.Movie();
            string session_id = Convert.ToString(Session["SessionId"]);
            //Models.User user = (Models.User)Session["user"]; PARA PASAR TODO EL MODELO ENTRE VISTAS
            if (session_id == "")
            {
                return RedirectToAction("Form", "Login");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://api.themoviedb.org/3/");

                    var responseTask = client.GetAsync("account/20013867/favorite/movies?api_key=c9617782375bf747c35d7aafecf16f5b&language=en-US&page=1&session_id=" + session_id);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        dynamic resultJSON = JObject.Parse(readTask.Result.ToString());

                        readTask.Wait();
                        popular.Movies = new List<object>();
                        string link = "http://image.tmdb.org/t/p/w500";

                        foreach (var resultItem in resultJSON.results)
                        {
                            ML.Movie popularItem = new ML.Movie();
                            popularItem.id = resultItem.id;
                            popularItem.original_title = resultItem.original_title;
                            popularItem.overview = resultItem.overview;
                            popularItem.backdrop_path = resultItem.backdrop_path;

                            popularItem.poster_path = link + resultItem.poster_path;
                            popular.Movies.Add(popularItem); //Agregar a el objeto principal la lista de peliculas
                        }
                    }

                }
                return View(popular);
            }
        }

       
        public JsonResult AddFavoritas(int id, bool Favorita)
        {
            ML.Favoritas favoritas = new ML.Favoritas();
            string session_id = Convert.ToString(Session["SessionId"]);
            favoritas.media_id = id;
            favoritas.favorite = Favorita;
            favoritas.media_type = "movie";

            if (session_id == "")
            {
                return Json("Form", "Login");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://api.themoviedb.org/3/");

                    var responseTask = client.PostAsync("account/20013867/favorite?api_key=c9617782375bf747c35d7aafecf16f5b&session_id=" + session_id, new StringContent(
                        new JavaScriptSerializer().Serialize(favoritas), Encoding.UTF8, "application/json")); //Enviar modelo serializar
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        dynamic resultJSON = JObject.Parse(readTask.Result.ToString());

                        readTask.Wait();
                        ViewBag.Mensaje = "";
                        return Json("Modal");

                    }

                }
                ViewBag.Mensaje = "Ocurrio un error";

                return Json("Modal");

            }
        }
    }
}