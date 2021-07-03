using SocialMovieManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace SocialMovieManagementApplication.Services.Business.Data
{
    public class SearchDAO
    {
        readonly string connectionStr = "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=SocialMovieManagement ;Integrated Security=True;";

        //public KeyValuePair<int, string> SearchUsers(string searchTerm)
        //{
        //    KeyValuePair<int, string> users = new KeyValuePair<int, string>();
        //    users.
        //    string query = "SELECT user_id, username FROM dbo.Users WHERE username LIKE '%" + searchTerm + "%'";

           
        //    return users;
        //}

        public Dictionary<int, string> SearchUsers(string searchTerm)
        {
            Dictionary<int, string> foundUsers = new Dictionary<int, string>();
 
            string query = "SELECT user_id, username FROM dbo.Users WHERE USERNAME LIKE '%" + searchTerm + "%'";

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
                        int userID = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        foundUsers.Add(userID, username);
                    }
                }
            }
            catch(SqlException e)
            {
                Debug.WriteLine("Error generated in retrieval. Details: " + e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return foundUsers;
        }
    }
}