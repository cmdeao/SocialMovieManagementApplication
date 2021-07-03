using SocialMovieManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialMovieManagementApplication.Services.Business;
using System.Web.Mvc;
using System.Globalization;
using System.Diagnostics;

namespace SocialMovieManagementApplication.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        public ActionResult Index()
        {
            ProfileService service = new ProfileService();
            UserProfile userProfile = service.RetrieveProfile(UserManagement.Instance._loggedUser.userID);
            ViewBag.Message = UserManagement.Instance._loggedUser.username;
            return View(userProfile);
        }

        public ActionResult UpdateProfileView()
        {
            List<string> countryList = new List<string>();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach(CultureInfo cInfo in cInfoList)
            {
                RegionInfo r = new RegionInfo(cInfo.LCID);
                if(!(countryList.Contains(r.EnglishName)))
                {
                    countryList.Add(r.EnglishName);
                }
            }

            countryList.Sort();
            ViewBag.CountryList = countryList;
            ProfileService service = new ProfileService();
            UserProfile profile = new UserProfile();
            profile = service.RetrieveProfile(UserManagement.Instance._loggedUser.userID);
            return View("UpdateProfile", profile);
        }

        [HttpPost]
        public ActionResult UpdateProfile(UserProfile profile)
        {
            profile.userID = UserManagement.Instance._loggedUser.userID;
            if (!ModelState.IsValid)
            {
                return View("UpdateProfile");
            }

            ProfileService service = new ProfileService();

            if(!service.CheckProfile(profile.userID))
            {
                service.AddProfile(profile);
                return RedirectToAction("Index", "UserProfile");
            }
            else
            {
                service.UpdateProfile(profile);
                return RedirectToAction("Index", "UserProfile");
            }
        }

        [HttpPost]
        public ActionResult ViewProfile(int id, string username)
        {
            ProfileService service = new ProfileService();
            UserProfile searchedProfile = service.RetrieveProfile(id);
            ViewBag.Message = username;
            return View("UserProfile", searchedProfile);
        }
    }
}