using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SocialMovieManagementApplication.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            SocialService service = new SocialService();
            List<SocialPostModel> retrievedPosts = service.RetrievePosts();
            List<CommentModel> retrievedComments = service.RetrieveComments();

            PostModel model = new PostModel();
            model.posts = retrievedPosts;
            model.comments = retrievedComments;

            return View("Index", model);
        }

        public ActionResult ViewFriendPosts()
        {
            SocialService service = new SocialService();
            List<SocialPostModel> friendPosts = service.FriendPosts(UserManagement.Instance._loggedUser.userID);
            List<CommentModel> comments = new List<CommentModel>();
            for(int i = 0; i < friendPosts.Count; i++)
            {
                List<CommentModel> newComments = service.FriendsPostComments(friendPosts[i].postID);
                comments.AddRange(newComments);
            }

            PostModel model = new PostModel();
            model.posts = friendPosts;
            model.comments = comments;

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult PostComment(string comment, int postID)
        {
            CommentModel postComment = new CommentModel();
            DateTime commentDate = DateTime.Now;

            postComment.postID = postID;
            postComment.commentText = comment;
            postComment.postedBy = UserManagement.Instance._loggedUser.userID;
            postComment.postedUsername = UserManagement.Instance._loggedUser.username;
            postComment.postedDate = commentDate;

            SocialService service = new SocialService();
            if(service.PostComment(postComment))
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult PostContent(string postContent)
        {
            SocialService service = new SocialService();
            SocialPostModel post = new SocialPostModel();
            DateTime postDate = DateTime.Now;
            post.postedBy = UserManagement.Instance._loggedUser.userID;
            post.postedUsername = UserManagement.Instance._loggedUser.username;
            post.postContent = postContent;
            post.datePosted = postDate;
            if (service.CreatePost(post))
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                return View("Error");
            }
            
        }
    }

    public class PostModel
    {
        public List<SocialPostModel> posts { get; set; }
        public List<CommentModel> comments { get; set; }
    }
}