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
    }
}