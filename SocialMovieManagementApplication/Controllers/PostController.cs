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
            Debug.WriteLine("Size of List: " + retrievedPosts.Count);

            List<SocialPostModel> posts = new List<SocialPostModel>();
            List<CommentModel> comments = new List<CommentModel>();

            for(int i = 0; i < 3; i++)
            {
                CommentModel comment = new CommentModel();
                comment.postID = i;
                comment.commentText = "testing " + i + " hi";
                comments.Add(comment);
            }
            for(int i = 0; i < 3; i++)
            {
                SocialPostModel post = new SocialPostModel();
                post.postedUsername = "DEV: " + i;
                post.postID = i;
                post.postContent = "THIS IS A TEST FOR CONTENT " + i;
                posts.Add(post);
            }
            CommentModel newComment = new CommentModel();
            newComment.postID = 1;
            newComment.commentText = "THIS IS A FINAL TEST";
            comments.Add(newComment);
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
                Debug.WriteLine("Post ID: " + friendPosts[i].postID);
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
                //return Content(String.Format("Posted by: {0}, Username: {1}, Content: {2}, Date: {3}", post.postedBy, post.postedUsername, post.postContent, post.datePosted));
                return RedirectToAction("Index", "Post");
            }
            else
            {
                return Content("FAILED TO INSERT POST!");
            }
            
        }
    }

    public class PostModel
    {
        public List<SocialPostModel> posts { get; set; }
        public List<CommentModel> comments { get; set; }
    }
}