using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Models
{
    public class UserProfile
    {
        public int userID { get; set; }
        public string country { get; set; }

        public int age { get; set; }

        //public string favoriteMovie { get; set; }

        public string userBio { get; set; }
    }
}