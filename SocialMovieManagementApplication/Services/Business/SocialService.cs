using SocialMovieManagementApplication.Models;
using SocialMovieManagementApplication.Services.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Services.Business
{
    public class SocialService
    {
        SocialDAO service = new SocialDAO();

        public bool CreatePost(SocialPostModel post)
        {
            return service.CreatePost(post);
        }

        public bool PostComment(CommentModel comment)
        {
            return service.PostComment(comment);
        }

        public List<SocialPostModel> RetrievePosts()
        {
            return service.RetrievePosts();
        }

        public List<CommentModel> RetrieveComments()
        {
            return service.RetrieveComments();
        }

        public List<SocialPostModel> FriendPosts(int userID)
        {
            return service.RetrieveFriendPosts(userID);
        }

        public List<CommentModel> FriendsPostComments(int postID)
        {
            return service.FriendPostComments(postID);
        }

        public bool DeleteComment(int commentID)
        {
            return service.AdminDeleteComment(commentID);
        }

        public bool DeletePost(int postID)
        {
            return service.AdminDeletePost(postID);
        }
    }
}