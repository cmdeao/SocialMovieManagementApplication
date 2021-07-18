using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialMovieManagementApplication.Controllers
{
    public class FriendsController : Controller
    {
        // GET: Friends
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewFriends()
        {
            FriendsService service = new FriendsService();
            List<FriendModel> friends = service.RetrieveFriends(UserManagement.Instance._loggedUser.userID);
            return View("FriendsList", friends);
        }

        public ActionResult SendFriendRequest(int id, string username)
        {
            FriendsService service = new FriendsService();
            if(service.SendFriendRequest(UserManagement.Instance._loggedUser.userID, id))
            {
                return RedirectToAction("ViewFriends", "Friends");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult ViewFriendRequests()
        {
            FriendsService service = new FriendsService();
            List<FriendModel> friendRequests = new List<FriendModel>();

            friendRequests = service.RetrieveFriendRequests(UserManagement.Instance._loggedUser.userID);
            return View("FriendRequests", friendRequests);
        }

        [HttpPost]
        public ActionResult ConfirmFriendRequest(int id)
        {
            FriendsService service = new FriendsService();
            
            if(service.ConfirmFriendRequest(UserManagement.Instance._loggedUser.userID, id))
            {
                return RedirectToAction("ViewFriends", "Friends");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DeclineFriendRequest(int id)
        {
            FriendsService service = new FriendsService();
            if(service.DeclineFriend(UserManagement.Instance._loggedUser.userID, id))
            {
                return RedirectToAction("ViewFriendRequests", "Friends");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult RemoveFriend(int id)
        {
            FriendsService service = new FriendsService();
            
            if(service.RemoveFriend(UserManagement.Instance._loggedUser.userID, id))
            {
                return RedirectToAction("ViewFriends", "Friends");
            }
            else
            {
                return View("Error");
            }
        }
    }
}