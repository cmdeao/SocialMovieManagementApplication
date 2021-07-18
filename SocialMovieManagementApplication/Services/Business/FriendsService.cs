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

        public bool SendFriendRequest(int userID, int friendID)
        {
            return service.SendFriendRequest(userID, friendID);
        }

        public List<FriendModel> RetrieveFriendRequests(int userID)
        {
            return service.ViewFriendRequests(userID);
        }

        public bool ConfirmFriendRequest(int userID, int friendID)
        {
            return service.ConfirmFriendRequest(userID, friendID);
        }

        public bool RemoveFriend(int userID, int friendID)
        {
            return service.RemoveFriend(userID, friendID);
        }

        public bool CheckRequest(int userID, int friendID)
        {
            return service.CheckRequest(userID, friendID);
        }

        public bool ConfirmFriend(int userID, int friendID)
        {
            return service.ConfirmFriend(userID, friendID);
        }

        public bool DeclineFriend(int userID, int friendID)
        {
            return service.DeclineFriend(userID, friendID);
        }
    }
}