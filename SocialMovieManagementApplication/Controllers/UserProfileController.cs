using SocialMovieManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialMovieManagementApplication.Services.Business;
using System.Web.Mvc;
using System.Globalization;
using System.Diagnostics;
using Newtonsoft.Json;

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

        public ActionResult ViewUserCollection(UserProfile user)
        {
            int userID = user.userID;
            Debug.WriteLine("USER ID:" + userID);
            MovieService service = new MovieService();
            string jsonData = service.RetrieveMovies(userID);
            List<Search> jsonMovies = new List<Search>();

            if(jsonData != null)
            {
                jsonMovies = JsonConvert.DeserializeObject<List<Search>>(jsonData);
                ViewBag.Message = String.Format("Collection size: {0}", jsonMovies.Count);   
                return View("UserCollection", jsonMovies);
            }
            else
            {
                ViewBag.Message = String.Format("Collection size: 0");
                return View("UserCollection", jsonMovies);
            }
        }

        public ActionResult ViewCollection()
        {
            MovieService service = new MovieService();
            string jsonData = service.RetrieveMovies(UserManagement.Instance._loggedUser.userID);
            Debug.WriteLine("Data found: " + jsonData);
            List<Search> jsonMovies = new List<Search>();
            if (jsonData != null)
            {
                jsonMovies = JsonConvert.DeserializeObject<List<Search>>(jsonData);
                ViewBag.Message = String.Format("You have {0} movies in your collection!", jsonMovies.Count);
                return View("ProfileCollection", jsonMovies);
            }
            else
            {
                ViewBag.Message = String.Format("You have {0} movies in your collection!", 0);
                return View("ProfileCollection", jsonMovies);
            }
        }

        public ActionResult AddToCollection(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;

            MovieService service = new MovieService();
            if(service.CheckUserCollection(userID))
            {
                string jsonCollection = service.RetrieveMovies(userID);
                List<Search> retrievedMovies = JsonConvert.DeserializeObject<List<Search>>(jsonCollection);
                retrievedMovies.Add(item);
                string jsonInsertion = JsonConvert.SerializeObject(retrievedMovies, Formatting.Indented);

                if (service.UpdateCollection(userID, jsonInsertion))
                {
                    return RedirectToAction("ViewCollection", "UserProfile");
                }
                else
                {
                    return Content("SOMETHING WENT WRONG UPDATING A COLLECTION!");
                }
            }
            else
            {
                List<Search> newMovie = new List<Search>();
                newMovie.Add(item);
                string jsonData = JsonConvert.SerializeObject(newMovie, Formatting.Indented);
                if (service.InsertMovies(userID, jsonData))
                {
                    return RedirectToAction("ViewCollection", "UserProfile");
                }
                else
                {
                    return Content("SOMETHING WENT WRONG CREATING A COLLECTION!");
                }
            }
        }

        public ActionResult RemoveMovie(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;
            MovieService service = new MovieService();
            string jsonCollection = service.RetrieveMovies(userID);
            List<Search> retrievedMovies = JsonConvert.DeserializeObject<List<Search>>(jsonCollection);

            retrievedMovies.RemoveAll(x => x.Title == item.Title);
            string jsonUpdate = JsonConvert.SerializeObject(retrievedMovies, Formatting.Indented);
            if (service.UpdateCollection(userID, jsonUpdate))
            {
                return RedirectToAction("ViewCollection", "UserProfile");
            }
            else
            {
                return Content("SOMETHING WENT WRONG UPDATING A COLLECTION!");
            }
        }

        public async System.Threading.Tasks.Task<ActionResult> RandomMovie()
        {
            int userID = UserManagement.Instance._loggedUser.userID;

            MovieService service = new MovieService();
            string jsonCollection = service.RetrieveMovies(userID);
            List<Search> retrievedMovies = JsonConvert.DeserializeObject<List<Search>>(jsonCollection);

            var random = new Random();
            int randomIndex = random.Next(retrievedMovies.Count);

            Search randomMovie = retrievedMovies[randomIndex];
            MovieModel randomMovieDetails = await service.SearchMovieAPI(randomMovie.imdbID);

            return View("RandomMovieDetails", randomMovieDetails);
        }
    }
}