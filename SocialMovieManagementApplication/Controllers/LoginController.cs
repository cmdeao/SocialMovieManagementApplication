using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/* Social Movie Management Applicaiton
 * Version 0.1
 * Cameron Deao
 * 6/25/2021
 * The LoginController class handles operations for redirecting
 * users to the appropriate views through the ActionResult returns,
 * and passing established models into the appropriate services for
 * various operations.
 */

namespace SocialMovieManagementApplication.Controllers
{
    public class LoginController : Controller
    {
        // Index method returns the login page for the user.
        [HttpGet]
        public ActionResult Index()
        {
            return View("Login");
        }

        //Login method takes the passed UserLoginModel and passes it into
        //the appropriate service class. Based on the result of operations
        //within the service class, the appropriate view will be retuned
        //through the ActionResult return of the method.
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            //Checking if the passed model is valid and has no errors.
            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            //Establishing an instance of the SecurityService class.
            SecurityService service = new SecurityService();
            
            //Passing the established model into the security service for 
            //authentication and returning the appropriate view.
            if(service.Authenticate(model))
            {
                return View("LoginPassed");
            }
            else
            {
                return View("LoginFailed");
            }
        }
    }
}