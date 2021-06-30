using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Services.Business
{
    public class ProfileService
    {
        ProfileDAO service = new ProfileDAO();

        public UserProfile RetrieveProfile(int userID)
        {
            return service.RetrieveProfile(userID);
        }

        public bool AddProfile(UserProfile profile)
        {
            return service.AddProfile(profile);
        }

        public bool UpdateProfile(UserProfile profile)
        {
            return service.UpdateProfile(profile);
        }

        public bool CheckProfile(int userID)
        {
            return service.CheckProfile(userID);
        }
    }
}