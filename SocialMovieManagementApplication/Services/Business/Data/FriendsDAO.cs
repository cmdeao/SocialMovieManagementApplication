using SocialMovieManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Services.Business.Data
{
    public class FriendsDAO
    {
        readonly string connectionStr = "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=SocialMovieManagement ;Integrated Security=True;";

        public List<FriendModel> RetrieveFriends(int userID)
        {
            string query = "SELECT first_name, last_name, username, friend_id FROM Users m RIGHT JOIN Friends f ON f.friend_id = m.user_id WHERE f.user_id = " + userID;
            List<FriendModel> friends = new List<FriendModel>();
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
                        FriendModel friend = new FriendModel();
                        friend.firstName = reader.GetString(0);
                        friend.lastName = reader.GetString(1);
                        friend.username = reader.GetString(2);
                        friend.friendID = reader.GetInt32(3);
                        friends.Add(friend);
                    }
                }

                return friends;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public bool SendFriendRequest(int userID, int friendID)
        {
            string query = "INSERT INTO dbo.FriendRequests (userID, friendID) VALUES (@userID, @friendID)";
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                command.Parameters.Add("@userID", System.Data.SqlDbType.Int).Value = userID;
                command.Parameters.Add("@friendID", System.Data.SqlDbType.Int).Value = friendID;

                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        public bool ConfirmFriendRequest(int userID, int friendID)
        {
            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            SqlTransaction transaction;
            transaction = conn.BeginTransaction("ConfirmFriendTransaction");

            command.Connection = conn;
            command.Transaction = transaction;

            try
            {
                command.CommandText = "DELETE FROM dbo.FriendRequests WHERE friendID = " + userID + " AND userID = " + friendID;
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dbo.Friends (user_id, friend_id) VALUES (" + userID + ", " + friendID + ")";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dbo.Friends (user_id, friend_id) VALUES (" + friendID + ", " + userID + ")";
                command.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.GetType());
                Debug.WriteLine(e.Message);
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch(Exception e2)
                {
                    Debug.WriteLine(e2.GetType());
                    Debug.WriteLine(e2.Message);
                }
            }
            return false;
        }

        public List<FriendModel> ViewFriendRequests(int userID)
        {
            string query = "SELECT first_name, last_name, username, userID FROM FriendRequests f " +
                "RIGHT JOIN Users m ON m.user_id = f.userID WHERE f.friendID = " + userID;

            List<FriendModel> friendRequests = new List<FriendModel>();

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
                        FriendModel friendRequest = new FriendModel();
                        friendRequest.firstName = reader.GetString(0);
                        friendRequest.lastName = reader.GetString(1);
                        friendRequest.username = reader.GetString(2);
                        friendRequest.friendID = reader.GetInt32(3);
                        friendRequests.Add(friendRequest);
                    }
                }

                return friendRequests;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public bool RemoveFriend(int userID, int friendID)
        {
            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            SqlTransaction transaction;
            transaction = conn.BeginTransaction("RemoveFriend");

            command.Connection = conn;
            command.Transaction = transaction;

            try
            {
                command.CommandText = "DELETE FROM dbo.Friends WHERE user_id = " + userID + " AND friend_id = " + friendID;
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM dbo.Friends WHERE user_id = " + friendID + " AND friend_id = " + userID;
                command.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.GetType());
                Debug.WriteLine(e.Message);
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch(Exception e2)
                {
                    Debug.WriteLine(e2.GetType());
                    Debug.WriteLine(e2.Message);
                }
            }
            return false;
        }

        public bool CheckRequest(int userID, int friendID)
        {
            string query = "SELECT * FROM dbo.FriendRequests WHERE userID = " + userID + " AND friendID = " + friendID;
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return false;
        }

        public bool ConfirmFriend(int userID, int friendID)
        {
            string query = "SELECT * FROM dbo.Friends WHERE user_id = " + userID + " AND friend_id = " + friendID;
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return false;
        }

        public bool DeclineFriend(int userID, int friendID)
        {
            string query = "DELETE FROM dbo.FriendRequests WHERE friendID = " + userID + " AND userID = " + friendID;
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }
    }
}