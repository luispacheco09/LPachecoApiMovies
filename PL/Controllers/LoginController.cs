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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(ML.User user)
        {
            using (var token = new HttpClient())
            {

                token.BaseAddress = new Uri("http://api.themoviedb.org/3/");
                var responseTask = token.GetAsync("authentication/token/new?api_key=c9617782375bf747c35d7aafecf16f5b");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result);

                    user.request_token = resultJSON.request_token; //Se guarda el request token 

                    using (var token2 = new HttpClient())
                    {
                        token2.BaseAddress = new Uri("http://api.themoviedb.org/3/");
                       
                        var responseTask2 = token2.PostAsync("authentication/token/validate_with_login?api_key=c9617782375bf747c35d7aafecf16f5b", new StringContent(
                            new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json"));
                        responseTask2.Wait();
                        var result2 = responseTask2.Result;
                        if (result2.IsSuccessStatusCode)
                        {
                            var readTask2 = result2.Content.ReadAsStringAsync();
                            dynamic resultJSON2 = JObject.Parse(readTask2.Result);

                            user.request_token = resultJSON2.request_token;// se guarda el token

                            using (var token3 = new HttpClient())
                            {
                                token3.BaseAddress = new Uri("http://api.themoviedb.org/3/");
                                //var responseTask3 = token3.PostAsJsonAsync("authentication/session/new?api_key=1be03c502e7df602711a6414f7fd3f80", user);
                                var responseTask3 = token3.PostAsync("authentication/session/new?api_key=c9617782375bf747c35d7aafecf16f5b", new StringContent(
                                    new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json"));
                                responseTask3.Wait();
                                var result3 = responseTask3.Result;
                                if (result3.IsSuccessStatusCode)
                                {
                                    var readTask3 = result3.Content.ReadAsStringAsync();
                                    dynamic resultJSON3 = JObject.Parse(readTask3.Result);//guarda el token

                                    user.session_id = resultJSON3.session_id;
                                    Session["User"] = user;  //Boxing //MODELO
                                    Session["SessionId"] = user.session_id; //Dato Session
                                                                            //Session["Username"] = user.username; // Dato UserName

                                    //.Net Framework
                                    //Datos Primitivos // OBJETO

                                    //.Net Core
                                    //Solo pueden guardar int y string
                                    // Objeto complejo = Serilizar objeto --- JSON ""
                                    // Desarialziar JSON ---- Objeto
                                    return RedirectToAction("Favoritas", "Movie");
                                }
                            }
                        }
                    }
                }
                ViewBag.Mensaje = "Ha ocurrido un error";
                return View("Modal");
                //return RedirectToAction("Popular", "Movie");
            }
        }
    }
}