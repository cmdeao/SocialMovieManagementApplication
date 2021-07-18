using SocialMovieManagementApplication.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace SocialMovieManagementApplication.Services.Business.Data
{

    public class SocialDAO
    {
        readonly string connectionStr = "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=SocialMovieManagement ;Integrated Security=True;";
        public bool CreatePost(SocialPostModel post)
        {
            string query = "INSERT INTO dbo.SocialPosts(postedBy, postedUsername, postContent, datePosted) VALUES (@postedBy, @username, @content, @date)";

            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@postedBy", SqlDbType.Int).Value = post.postedBy;
                    sqlCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = post.postedUsername;
                    sqlCommand.Parameters.Add("@content", SqlDbType.Text).Value = post.postContent;
                    sqlCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = post.datePosted;

                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        return true;
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }

            return false;
        }

        public List<SocialPostModel> RetrievePosts()
        {
            string query = "SELECT * FROM dbo.SocialPosts ORDER BY datePosted DESC";
            List<SocialPostModel> posts = new List<SocialPostModel>();
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        SocialPostModel post = new SocialPostModel();
                        post.postID = reader.GetInt32(0);
                        post.postedBy = reader.GetInt32(1);
                        post.postedUsername = reader.GetString(2);
                        post.postContent = reader.GetString(3);
                        post.datePosted = reader.GetDateTime(4);
                        posts.Add(post);
                    }
                }

                return posts;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        public List<CommentModel> RetrieveComments()
        {
            string query = "SELECT * FROM dbo.SocialComments ORDER BY postedDate DESC";
            List<CommentModel> comments = new List<CommentModel>();
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        CommentModel comment = new CommentModel();
                        comment.commentID = reader.GetInt32(0);
                        comment.postID = reader.GetInt32(1);
                        comment.commentText = reader.GetString(2);
                        comment.postedBy = reader.GetInt32(3);
                        comment.postedDate = reader.GetDateTime(4);
                        comment.postedUsername = reader.GetString(5);
                        comments.Add(comment);
                    }
                }

                return comments;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public bool PostComment(CommentModel comment)
        {
            string query = "INSERT INTO dbo.SocialComments (postID, commentText, postedBy, postedDate, commentUsername) VALUES (@id, @text, @postedBy, @date, @username)";

            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = comment.postID;
                    sqlCommand.Parameters.Add("@text", SqlDbType.Text).Value = comment.commentText;
                    sqlCommand.Parameters.Add("@postedBy", SqlDbType.Int).Value = comment.postedBy;
                    sqlCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = comment.postedDate;
                    sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = comment.postedUsername;

                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        return true;
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            return false;
        }

        public List<SocialPostModel> RetrieveFriendPosts(int userID)
        {
            string query = "SELECT p.* FROM Users m RIGHT JOIN Friends f ON f.friend_id = m.user_id LEFT JOIN SocialPosts p ON p.postedBy = f.friend_id" +
                " WHERE f.user_id = " + userID + " AND p.postedBy IS NOT NULL ORDER BY p.datePosted DESC";

            List<SocialPostModel> friendsPosts = new List<SocialPostModel>();
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        SocialPostModel post = new SocialPostModel();
                        post.postID = reader.GetInt32(0);
                        post.postedBy = reader.GetInt32(1);
                        post.postedUsername = reader.GetString(2);
                        post.postContent = reader.GetString(3);
                        post.datePosted = reader.GetDateTime(4);
                        friendsPosts.Add(post);
                    }
                }

                return friendsPosts;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        public List<CommentModel> FriendPostComments(int postID)
        {
            string query = "SELECT * FROM SocialComments WHERE postID = " + postID + " ORDER BY postedDate DESC";
            List<CommentModel> comments = new List<CommentModel>();
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        CommentModel comment = new CommentModel();
                        comment.commentID = reader.GetInt32(0);
                        comment.postID = reader.GetInt32(1);
                        comment.commentText = reader.GetString(2);
                        comment.postedBy = reader.GetInt32(3);
                        comment.postedDate = reader.GetDateTime(4);
                        comment.postedUsername = reader.GetString(5);
                        comments.Add(comment);
                    }
                }

                return comments;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }
    }
}