using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}