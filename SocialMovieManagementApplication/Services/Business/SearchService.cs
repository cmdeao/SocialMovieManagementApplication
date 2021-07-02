using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Services.Business
{
    public class SearchService
    {
        SearchDAO service = new SearchDAO();

        public Dictionary<int, string> SearchUsers(string searchTerm)
        {
            return service.SearchUsers(searchTerm);
        }
    }
}