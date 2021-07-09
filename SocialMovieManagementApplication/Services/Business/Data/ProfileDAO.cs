using SocialMovieManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics;
using SocialMovieManagementApplication.Controllers;
using Newtonsoft.Json;

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

            string query = "INSERT INTO dbo.UserProfiles(user_id, country, age, user_bio) VALUES (@userID, @country, @age, @userBio)";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int)).Value = UserManagement.Instance._loggedUser.userID;
            command.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar, 100)).Value = profile.country;
            command.Parameters.Add(new SqlParameter("@age", SqlDbType.Int)).Value = profile.age;
            //command.Parameters.Add(new SqlParameter("@favMovie", SqlDbType.VarChar, 100)).Value = profile.favoriteMovie;
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
            string query = "UPDATE dbo.UserProfiles SET country=@country, age=@age, user_bio=@bio WHERE user_id=@id";

            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);
            int retValue = 0;

            try
            {
                conn.Open();

                command.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar, 100)).Value = profile.country;
                command.Parameters.Add(new SqlParameter("@age", SqlDbType.Int)).Value = profile.age;
                //command.Parameters.Add(new SqlParameter("@favMovie", SqlDbType.VarChar, 100)).Value = profile.favoriteMovie;
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
                        //profile.favoriteMovie = reader.GetString(4);
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

        public bool AddFavoriteMovie(string jsonData, int userID)
        {
            bool operationSuccess = false;
            string query = "UPDATE dbo.UserProfiles SET favorite_movie=@json WHERE user_id=@userID";
            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@json", SqlDbType.Text).Value = jsonData;
                    sqlCommand.Parameters.Add("@userID", SqlDbType.Int).Value = userID;

                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        operationSuccess = true;
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            return operationSuccess;
        }

        public Search RetrieveFavoriteMovie(int userID)
        {
            string query = "SELECT favorite_movie FROM dbo.UserProfiles WHERE user_id=@userID AND favorite_movie IS NOT NULL";
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        string json = reader.GetString(0);
                        List<Search> movie = JsonConvert.DeserializeObject<List<Search>>(json);
                        reader.Close();
                        conn.Close();
                        return movie[0];
                    }
                }
            }
            catch(SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }
    }
}