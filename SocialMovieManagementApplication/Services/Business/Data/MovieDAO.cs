using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace SocialMovieManagementApplication.Services.Business.Data
{
    public class MovieDAO
    {
        readonly string connectionStr = "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=SocialMovieManagement ;Integrated Security=True;";
        public bool InsertMovieCollection(int userID, string jsonData)
        {
            bool operationSuccess = false;

            string query = "INSERT INTO dbo.Collections (user_id, collection_data) VALUES (@userid, @CollectionString)";

            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@userid", SqlDbType.Int).Value = userID;
                    sqlCommand.Parameters.Add("@CollectionString", SqlDbType.Text).Value = jsonData;

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

        public string RetrieveMovies(int userid)
        {
            string returnValue = null;
            string query = "SELECT collection_data FROM dbo.Collections WHERE user_id = @userID";

            SqlConnection conn = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@userID", SqlDbType.Int).Value = userid;
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    if(reader.Read())
                    {
                        returnValue = reader.GetString(0);
                        reader.Close();
                        conn.Close();
                    }
                }
            }
            catch(SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }

            return returnValue;
        }

        public bool UpdateUserCollection(int userID, string jsonData)
        {
            string query = "UPDATE dbo.Collections SET collection_data=@CollectionData WHERE user_id=@id";
            bool operationSuccess = false;

            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@CollectionData", SqlDbType.Text).Value = jsonData;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = userID;

                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        operationSuccess = true;
                    }
                    catch(SqlException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }

            //SqlConnection conn = new SqlConnection(connectionStr);
            //SqlCommand command = new SqlCommand(query, conn);
            //int retValue = 0;

            //try
            //{
            //    conn.Open();

            //    command.Parameters.Add(new SqlParameter("@CollectionData", SqlDbType.Text)).Value = jsonData;
            //    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = userID;
            //    command.Prepare();

            //    retValue = command.ExecuteNonQuery();
            //    conn.Close();

            //    if (retValue == 0)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //catch(SqlException e)
            //{
            //    Debug.WriteLine(e.Message);
            //}
            //finally
            //{
            //    conn.Close();
            //}
            return operationSuccess;
        }

        public bool CheckCollection(int userID)
        {
            string query = "SELECT * FROM dbo.Collections WHERE user_id = @id";
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
                Debug.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }

            return false;
        }
    }
}