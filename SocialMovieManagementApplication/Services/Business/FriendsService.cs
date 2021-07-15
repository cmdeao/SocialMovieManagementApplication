using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Services.Business
{
    public class FriendsService
    {
        FriendsDAO service = new FriendsDAO();
        public List<FriendModel> RetrieveFriends(int userID)
        {
            return service.RetrieveFriends(userID);
        }
    }
}