using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMovieManagementApplication.Services.Business;

namespace SocialMovieManagementApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(UserManagement.Instance._loggedUser != null)
            {
                ViewBag.Message = String.Format("Welcome to the application {0}!",
                    UserManagement.Instance._loggedUser.username);
            }
            else
            {
                ViewBag.Message = "Welcome to the application! Please login to fully" +
                    " utilize the applications features!";
            }
            return View("Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "About the application!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Testing()
        {
            string test = "This is a hello world test!";

            return Content(test);
        }

        [HttpPost]
        public ActionResult Search(string SearchText)
        {
            Debug.WriteLine("Hello world!");
            Dictionary<int, string> users = new Dictionary<int, string>();
            SearchService service = new SearchService();
            users = service.SearchUsers(SearchText);

            Debug.WriteLine("Size of Dict: " + users.Count);
            foreach(KeyValuePair<int, string> element in users)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", element.Key, element.Value);
            }
            //return Content(SearchText);
            return View("SearchResults", users);
        }
    }
}