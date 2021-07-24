using Newtonsoft.Json;
using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SocialMovieManagementApplication.Services.Business
{
    public class MovieService
    {
        MovieDAO service = new MovieDAO();
        public bool InsertMovies(int userID, string jsonData)
        {
            return service.InsertMovieCollection(userID, jsonData);
        }

        public string RetrieveMovies(int userID)
        {
            return service.RetrieveMovies(userID);
        }

        public bool CheckUserCollection(int userID)
        {
            return service.CheckCollection(userID);
        }

        public bool UpdateCollection(int userID, string jsonData)
        {
            return service.UpdateUserCollection(userID, jsonData);
        }

        public bool CheckWishlist(int userID)
        {
            return service.CheckWishList(userID);
        }

        public bool UpdateWishlist(int userID, string jsonData)
        {
            return service.UpdateMovieWishlist(userID, jsonData);
        }

        public string RetrieveWishList(int userID)
        {
            return service.RetrieveWishList(userID);
        }

        public bool CreateWishlist(int userID, string jsonData)
        {
            return service.CreateWishLIst(userID, jsonData);
        }
        public async System.Threading.Tasks.Task<MovieModel> SearchMovieAPI(string id)
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
    }
}