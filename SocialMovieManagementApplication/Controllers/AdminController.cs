using SocialMovieManagementApplication.Services.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMovieManagementApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeleteComment(int commentID)
        {
            SocialService service = new SocialService();
            if(service.DeleteComment(commentID))
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeletePost(int postID)
        {
            SocialService service = new SocialService();
            if (service.DeletePost(postID))
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                return View("Error");
            }
        }
    }
}