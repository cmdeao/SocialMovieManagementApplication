using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business;


namespace SocialMovieManagementApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(UserManagement.Instance._loggedUser != null)
            {
                ViewBag.Message = String.Format("Welcome to the application {0}!",
                    UserManagement.Instance._loggedUser.username);
            }
            else
            {
                ViewBag.Message = "Welcome to the application! Please login to fully" +
                    " utilize the applications features!";
            }

            List<Search> movies = new List<Search>();
            Search firstMovie = new Search();
            firstMovie.Title = "The Godfather";
            firstMovie.Year = "1977";
            firstMovie.imdbID = "12345";
            firstMovie.Type = "movie";
            firstMovie.Poster = "posterLINK";
            movies.Add(firstMovie);

            Search secondMovie = new Search();
            secondMovie.Title = "Goodfellas";
            secondMovie.Year = "1999";
            secondMovie.imdbID = "98765";
            secondMovie.Type = "movie";
            secondMovie.Poster = "LINKposter";
            movies.Add(secondMovie);

            Search thirdMovie = new Search();
            thirdMovie.Title = "Willy Wonka";
            thirdMovie.Year = "1969";
            thirdMovie.imdbID = "654321";
            thirdMovie.Type = "game";
            thirdMovie.Poster = "POSTERINFO";
            movies.Add(thirdMovie);

            string json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            Debug.WriteLine("JSON OUTPUT");
            Debug.WriteLine(json);

            //MovieService service = new MovieService();

            //if(service.CheckUserCollection(UserManagement.Instance._loggedUser.userID))
            //{
            //    if(service.UpdateCollection(UserManagement.Instance._loggedUser.userID, json))
            //    {
            //        Debug.WriteLine("UPDATED DATA!");
            //    }
            //    else
            //    {
            //        Debug.WriteLine("SOMETHING WENT WRONG WITH THE UPDATE!");
            //    }
            //}
            //else
            //{
            //    service.InsertMovies(UserManagement.Instance._loggedUser.userID, json);
            //}

            //service.InsertMovies(UserManagement.Instance._loggedUser.userID, json);
            //string testing = service.RetrieveMovies(UserManagement.Instance._loggedUser.userID);
            //Debug.WriteLine("WE FOUND SOME MOVIES: " + testing);

            //List<Search> jsonMovies = JsonConvert.DeserializeObject<List<Search>>(testing);
            //for (int i = 0; i < jsonMovies.Count; i++)
            //{
            //    Debug.WriteLine(jsonMovies[i].Title + " " + jsonMovies[i].Year + " " + jsonMovies[i].imdbID + " " + jsonMovies[i].Type + " " + jsonMovies[i].Poster);
            //}


            //List<Search> jsonMovies = JsonConvert.DeserializeObject<List<Search>>(json);
            //for(int i = 0; i < jsonMovies.Count; i++)
            //{
            //    Debug.WriteLine(jsonMovies[i].Title + jsonMovies[i].Year + jsonMovies[i].imdbID + jsonMovies[i].Type + jsonMovies[i].Poster);
            //}
            //Debug.WriteLine("Size of list: " + jsonMovies.Count);

            //await CheckAPIAsync();

            return View("Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "About the application!";

            return View();
        }

        public async System.Threading.Tasks.Task CheckAPIAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://movie-database-imdb-alternative.p.rapidapi.com/?s=Avengers%20Endgame&page=1&r=json"),
                Headers =
                {
                    { "x-rapidapi-key", "05daa4efc9msh2a725e479ff0086p11704ejsn648b6e1fc2ca" },
                    { "x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                //MovieModel model = JsonConvert.DeserializeObject<MovieModel>(body);

                //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(body);
                //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(body);
                //Root myDeserializedClass = new Root();
                //myDeserializedClass = JsonConvert.DeserializeObject<Root>(body);
                //Debug.WriteLine("Count of list: " + myDeserializedClass.MovieSearchModel.Count);
                //Debug.WriteLine("FOUND MOVIES:");
                //for(int i = 0; i < myDeserializedClass.MovieSearchModel.Count; i++)
                //{
                //    Debug.WriteLine(myDeserializedClass.MovieSearchModel[i].Title);
                //}

                Debug.WriteLine("Old output!");
                Debug.WriteLine(body);
                //Console.WriteLine(body);
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Testing()
        {
            string test = "This is a hello world test!";
            return Content(test);
        }

        [HttpPost]
        public ActionResult Search(string SearchText)
        {
            Dictionary<int, string> users = new Dictionary<int, string>();
            SearchService service = new SearchService();
            users = service.SearchUsers(SearchText);
            return View("SearchResults", users);
        }
    }

    //public class Search
    //{
    //    public string Title { get; set; }
    //    public string Year { get; set; }
    //    public string imdbID { get; set; }
    //    public string Type { get; set; }
    //    public string Poster { get; set; }
    //}

    //public class Root
    //{
    //    public List<Search> Search { get; set; }
    //    public string totalResults { get; set; }
    //    public string Response { get; set; }
    //}
}