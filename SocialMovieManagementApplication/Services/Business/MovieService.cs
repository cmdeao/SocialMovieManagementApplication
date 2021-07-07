using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}