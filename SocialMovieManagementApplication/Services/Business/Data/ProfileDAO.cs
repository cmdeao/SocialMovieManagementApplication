using SocialMovieManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace SocialMovieManagementApplication.Services.Business.Data
{
    public class ProfileDAO
    {
        readonly string connectionStr = "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=SocialMovieManagement ;Integrated Security=True;";

        public bool AddProfile(UserProfile profile)
        {
            int retValue = 0;

            SqlConnection conn = new SqlConnection(connectionStr);
            conn.Open();

            string query = "INSERT INTO dbo.UserProfiles(user_id, country, age, favorite_movie, user_bio) " +
                "VALUES(@userID, @country, @age, @favMovie, @userBio)";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int)).Value = UserManagement.Instance._loggedUser.userID;
            command.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar, 100)).Value = profile.country;
            command.Parameters.Add(new SqlParameter("@age", SqlDbType.Int)).Value = profile.age;
            command.Parameters.Add(new SqlParameter("@favMovie", SqlDbType.VarChar, 100)).Value = profile.favoriteMovie;
            command.Parameters.Add(new SqlParameter("@userBio", SqlDbType.VarChar, 1000)).Value = profile.userBio;

            command.Prepare();

            retValue = command.ExecuteNonQuery();
            conn.Close();

            if(retValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool UpdateProfile(UserProfile profile)
        {
            string query = "UPDATE dbo.UserProfiles SET country=@country, age=@age, favorite_movie=@favMovie, user_bio=@bio" +
                " WHERE user_id=@id";
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);
            int retValue = 0;

            try
            {
                conn.Open();

                command.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar, 100)).Value = profile.country;
                command.Parameters.Add(new SqlParameter("@age", SqlDbType.Int)).Value = profile.age;
                command.Parameters.Add(new SqlParameter("@favMovie", SqlDbType.VarChar, 100)).Value = profile.favoriteMovie;
                command.Parameters.Add(new SqlParameter("@bio", SqlDbType.VarChar, 1000)).Value = profile.userBio;
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = profile.userID;
                command.Prepare();

                retValue = command.ExecuteNonQuery();
                conn.Close();

                if(retValue == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Error generated in update. Details: " + e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public UserProfile RetrieveProfile(int userID)
        {
            string query = "SELECT * FROM dbo.UserProfiles WHERE user_id = @id";

            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@id", SqlDbType.Int).Value = userID;
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        UserProfile profile = new UserProfile();
                        profile.userID = reader.GetInt32(1);
                        profile.country = reader.GetString(2);
                        profile.age = reader.GetInt32(3);
                        profile.favoriteMovie = reader.GetString(4);
                        profile.userBio = reader.GetString(5);

                        conn.Close();
                        return profile;
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
            return null;
        }
        
        public bool CheckProfile(int userID)
        {
            string query = "SELECT * FROM dbo.UserProfiles WHERE user_id = @id";
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@id", SqlDbType.Int).Value = userID;
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    return true;
                }
            }
            catch(SqlException e)
            {
                Debug.WriteLine("Error generated in retrieval. Details: {0}", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
    }
}