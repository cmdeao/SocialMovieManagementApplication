using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMovieManagementApplication.Controllers
{
    public class Test : Controller
    {
        // GET: Test
        public string Index()
        {
            //return View();
            return @"<b>Hello World as a string from Index</b>";
        }

        [HttpGet]
        public string Play()
        {
            return @"<b>Hello World as a string from Play</b>";
        }
    }
}