using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using SocialMovieManagementApplication.Models;

namespace SocialMovieManagementApplication.Controllers
{
    public class MovieSearchController : Controller
    {
        //Root myDeserializedClass = null;
        
        // GET: MovieSearch
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Index(string MovieSearchTerm)
        {
            if(String.IsNullOrEmpty(MovieSearchTerm))
            {
                return View("Error");
            }

            var uriBuilder = new UriBuilder("https://movie-database-imdb-alternative.p.rapidapi.com");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["s"] = MovieSearchTerm;
            parameters["page"] = "1";
            parameters["type"] = "movie";
            parameters["r"] = "json";
            uriBuilder.Query = parameters.ToString();

            Uri finalUrl = uriBuilder.Uri;
            var request = WebRequest.Create(finalUrl);

            string finalResult = Regex.Replace(finalUrl.ToString(), @"[+]+", "%20");
            Root myDeserializedClass = await CheckAPIAsync(finalResult);
            return View("MovieResults", myDeserializedClass.Search);
        }
        public async System.Threading.Tasks.Task<Root> CheckAPIAsync(string incSearch)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(incSearch),
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
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(body);
                return myDeserializedClass;
            }
        }

        public async System.Threading.Tasks.Task<MovieModel> SearchMovieAPIAsync(string id)
        {
            MovieModel foundMovie;
            string i = id;
            var uriBuilder = new UriBuilder("https://movie-database-imdb-alternative.p.rapidapi.com");
            var paramaeters = HttpUtility.ParseQueryString(string.Empty);
            paramaeters["i"] = i;
            paramaeters["type"] = "movie";
            paramaeters["r"] = "json";
            uriBuilder.Query = paramaeters.ToString();

            Uri finalUrl = uriBuilder.Uri;
            var newRequest = WebRequest.Create(finalUrl);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(finalUrl.ToString()),
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
                foundMovie = JsonConvert.DeserializeObject<MovieModel>(body);

                return foundMovie;
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ViewMovieAsync(string id)
        {
            MovieModel foundMovie = await SearchMovieAPIAsync(id);
            return View("ViewMovie",foundMovie);
        }
    }

    public class Search
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

    public class Root
    {
        public List<Search> Search { get; set; }
        public string totalResults { get; set; }
        public string Response { get; set; }
    }
}