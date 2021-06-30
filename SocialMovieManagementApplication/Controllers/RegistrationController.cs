using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business;

/* Social Movie Management Applicaiton
 * Version 0.1
 * Cameron Deao
 * 6/25/2021
 * The RegistrationController class handles operations for redirecting
 * users to the appropriate views through the ActionResult returns,
 * and passing established models into the appropriate services for
 * various operations.
 */

namespace SocialMovieManagementApplication.Controllers
{
    public class RegistrationController : Controller
    {
        //Index method returns the registration page for the user.
        public ActionResult Index()
        {
            return View("Registration");
        }

        //RegisterAccount method takes the passed UserModel and passes it into
        //the appropriate service class. Based on the result of operations 
        //within the service class, the appropriate view will be returned 
        //through the ActionResult return of the method.
        [HttpPost]
        public ActionResult RegisterAccount(UserModel model)
        {
            //Establishing an instance of the SecurityService class.
            SecurityService service = new SecurityService();

            //Checking if the submitted username exists within the database.
            if(service.CheckUser(model))
            {
                //Adding an error to the model.
                ModelState.AddModelError("Username", "Please enter a valid username.");
            }

            //Checking if the submitted email exists within the database.
            if(service.CheckEmail(model))
            {
                //Adding an error to the model.
                ModelState.AddModelError("emailAddress", "Please enter a valid email.");
            }

            //Checking if the passed model is valid and has no errors.
            if (!ModelState.IsValid)
            {
                return View("Registration");
            }

            //Passing the established model into the security service for 
            //registration and returning the appropriate view.
            if (service.Register(model))
            {
                return View("/Views/Login/Login.cshtml");
            }
            else
            {
                return View("Registration");
            }


            //THIS IS A TEST!
        }
    }
}