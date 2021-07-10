using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Models
{
    public class MovieSearchModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

    public class Root
    {
        public List<MovieSearchModel> MovieSearchModel { get; set; }
        public string totalResults { get; set; }
        public string Response { get; set; }
    }
}