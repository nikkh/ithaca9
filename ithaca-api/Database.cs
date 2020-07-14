using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ithaca_api
{
    public static class Database
    {

        public static ILogger Logger { get; set; }

        public static LuisAccount GetLuisAccount(string email)
        {
            if (Logger == null) { throw new Exception("I need a logger"); }
            var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            LuisAccount la = new LuisAccount();
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"select Id, Email, LuisAccountNumber, LuisFavoriteColor, EnableSelfRegistration from LuisRegistrations  where Email = '{email}'";
                command.Connection = connection;


                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        la.Id = Convert.ToInt32(reader["Id"]);
                        la.Email = Convert.ToString(reader["Email"]);
                        la.LuisAccountNumber = Convert.ToString(reader["LuisAccountNumber"]);
                        la.LuisFavoriteColor = Convert.ToString(reader["LuisFavoriteColor"]);
                        la.EnableSelfRegistration = Convert.ToBoolean(reader["EnableSelfRegistration"]);
                    }

                }
                catch (Exception e)
                {
                    Logger.LogError($"Exception while reading from database for email {email}, {e}");
                    return null;
                }
                finally
                {
                    reader.Close();
                }

            }
            return la;
        }
    }
}
