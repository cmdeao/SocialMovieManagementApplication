using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/* Social Movie Management Applicaiton
 * Version 0.1
 * Cameron Deao
 * 6/25/2021
 * The SecurityService class passes data into the SecurityDAO for operations
 * and returns the results of operations to the controller.
 */

namespace SocialMovieManagementApplication.Services.Business
{
    public class SecurityService
    {
        //Establishing an instance of the SecurityDAO class.
        SecurityDAO service = new SecurityDAO();

        //Authenticate returns true if passed login credentials are found
        //within the persistent database, and false if not.
        public bool Authenticate(UserLoginModel user)
        {
            return service.FindByUser(user);
        }

        //CheckUser method checks if a passed username exists within the 
        //persistent database.
        public bool CheckUser(UserModel user)
        {
            return service.CheckUsername(user);
        }

        //CheckEmail method checks if a passed email exists within the
        //persistent database.
        public bool CheckEmail(UserModel user)
        {
            return service.CheckEmail(user);
        }

        //Register method returns true if passed registration credentials
        //are properly inserted and stored within the persistent database,
        //and false if not.
        public bool Register(UserModel user)
        {
            return service.RegisterUser(user);
        }
    }
}