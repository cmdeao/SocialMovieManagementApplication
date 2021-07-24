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
using System.Dynamic;

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

            ProfileModel model = new ProfileModel();
            Search favoriteMovie = new Search();
            model.movie = service.RetrieveFavoriteMovie(UserManagement.Instance._loggedUser.userID);
            model.userModel = userProfile;
            return View(model);
        }

        public ActionResult UpdateProfileView()
        {
            List<string> countryList = new List<string>();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cInfo in cInfoList)
            {
                RegionInfo r = new RegionInfo(cInfo.LCID);
                if (!(countryList.Contains(r.EnglishName)))
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

            if (!service.CheckProfile(profile.userID))
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
            FriendsService friendService = new FriendsService();
            UserProfile searchedProfile = service.RetrieveProfile(id);
            Search favoriteMovie = service.RetrieveFavoriteMovie(id);
            
            ProfileModel model = new ProfileModel();
            model.friendRequest = false;
            if (friendService.ConfirmFriend(UserManagement.Instance._loggedUser.userID, id))
            {
                model.friend = true;
            }
            else
            {
                bool pendingRequest = friendService.CheckRequest(UserManagement.Instance._loggedUser.userID, id);
                model.friendRequest = pendingRequest;
            }
            model.userModel = searchedProfile;
            model.movie = favoriteMovie;
            ViewBag.Message = username;
            return View("UserProfile", model);
        }

        public ActionResult ViewUserCollection(UserProfile user)
        {
            int userID = user.userID;
            MovieService service = new MovieService();
            string jsonData = service.RetrieveMovies(userID);
            List<Search> jsonMovies = new List<Search>();

            if (jsonData != null)
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

        public ActionResult ViewUserWishlist(UserProfile user)
        {
            int userID = user.userID;
            MovieService service = new MovieService();
            string jsonData = service.RetrieveWishList(userID);
            List<Search> jsonWishlist = new List<Search>();

            if (jsonData != null)
            {
                jsonWishlist = JsonConvert.DeserializeObject<List<Search>>(jsonData);
                ViewBag.Message = String.Format("Wishlist size: {0}", jsonWishlist.Count);
                return View("UserWishlist", jsonWishlist);
            }
            else
            {
                ViewBag.Message = "Collection size: 0";
                return View("UserWishlist", jsonWishlist);
            }
        }

        public ActionResult ViewCollection()
        {
            MovieService service = new MovieService();
            string jsonData = service.RetrieveMovies(UserManagement.Instance._loggedUser.userID);
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

        public ActionResult ViewWishlist()
        {
            MovieService service = new MovieService();
            string jsonData = service.RetrieveWishList(UserManagement.Instance._loggedUser.userID);
            List<Search> jsonWish = new List<Search>();

            if(jsonData != null)
            {
                jsonWish = JsonConvert.DeserializeObject<List<Search>>(jsonData);
                ViewBag.Message = String.Format("You have {0} movies in your wishlist.", jsonWish.Count);
                return View("ProfileWishlist", jsonWish);
            }
            else
            {
                ViewBag.Message = "You have 0 movies in your wishlist";
                return View("ProfileWishList", jsonWish);
            }
        }

        public ActionResult AddToCollection(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;

            MovieService service = new MovieService();
            if (service.CheckUserCollection(userID))
            {
                string jsonCollection = service.RetrieveMovies(userID);
                List<Search> retrievedMovies = JsonConvert.DeserializeObject<List<Search>>(jsonCollection);

                if (retrievedMovies.Any(x => x.imdbID == item.imdbID))
                {
                    return View("CollectionError");
                }

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
                return View("Error");
            }
        }

        public ActionResult RemoveFromWishlist(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;
            MovieService service = new MovieService();
            string jsonWishlist = service.RetrieveWishList(userID);
            List<Search> retrievedWishlist = JsonConvert.DeserializeObject<List<Search>>(jsonWishlist);

            retrievedWishlist.RemoveAll(x => x.imdbID == item.imdbID);
            string jsonUpdate = JsonConvert.SerializeObject(retrievedWishlist, Formatting.Indented);
            if(service.UpdateWishlist(userID, jsonUpdate))
            {
                return RedirectToAction("ViewWishlist", "UserProfile");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult AddToWishlist(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;

            MovieService service = new MovieService();

            if(!service.CheckUserCollection(userID))
            {
                List<Search> wishList = new List<Search>();
                wishList.Add(item);
                string jsonWishlist = JsonConvert.SerializeObject(wishList, Formatting.Indented);
                if(service.CreateWishlist(userID, jsonWishlist))
                {
                    return RedirectToAction("ViewWishlist", "UserProfile");
                }
                else
                {
                    return View("Error");
                }
            }

            if (service.CheckWishlist(userID))
            {
                string jsonWishlist = service.RetrieveWishList(userID);
                List<Search> wishlist = JsonConvert.DeserializeObject<List<Search>>(jsonWishlist);

                if (wishlist.Any(x => x.imdbID == item.imdbID))
                {
                    return View("CollectionError");
                }

                wishlist.Add(item);
                string wishlistInsertion = JsonConvert.SerializeObject(wishlist, Formatting.Indented);

                if (service.UpdateWishlist(userID, wishlistInsertion))
                {
                    return RedirectToAction("ViewWishlist", "UserProfile");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                List<Search> newWishlist = new List<Search>();
                newWishlist.Add(item);
                string jsonWishlist = JsonConvert.SerializeObject(newWishlist, Formatting.Indented);
                if (service.UpdateWishlist(userID, jsonWishlist))
                {
                    return RedirectToAction("ViewWishlist", "UserProfile");
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult AddWishlistItemToCollection(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;
            MovieService service = new MovieService();
            string jsonWishlist = service.RetrieveWishList(userID);
            List<Search> retrievedWishlist = JsonConvert.DeserializeObject<List<Search>>(jsonWishlist);

            retrievedWishlist.RemoveAll(x => x.imdbID == item.imdbID);
            string jsonUpdate = JsonConvert.SerializeObject(retrievedWishlist, Formatting.Indented);
            if (service.UpdateWishlist(userID, jsonUpdate))
            {
                return AddToCollection(item);
            }
            else
            {
                return ErrorPage();
            }
        }

        public ActionResult ErrorPage()
        {
            return View("Error");
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

        public ActionResult SetFavoriteMovie(Search item)
        {
            int userID = UserManagement.Instance._loggedUser.userID;
            List<Search> movie = new List<Search>();
            movie.Add(item);
            string json = JsonConvert.SerializeObject(movie, Formatting.Indented);
            ProfileService service = new ProfileService();
            service.AddFavoriteMovie(json, userID);
            return RedirectToAction("Index", "UserProfile");
        }
    }

    public class ProfileModel
    {
        public UserProfile userModel {get; set;}
        public Search movie { get; set; }
        public bool friendRequest { get; set; }
        public bool friend { get; set; }
    }
}