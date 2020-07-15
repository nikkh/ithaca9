using Microsoft.Extensions.Logging; 
using System;
using System.Collections.Generic;
using System.Text;

namespace ithaca_api
{

    public static class Authenticator
    {
        static string validApiUser = Environment.GetEnvironmentVariable("B2C_1A_LuisRestUserName");
        static string validApiPassword = Environment.GetEnvironmentVariable("B2C_1A_LuisRestUserPassword");
        public static ILogger log { get; set; }
        public static bool Authenticate(string base64Credentials)
        {
            if (log == null) { throw new Exception("I need a logger"); }
            if (validApiUser == null) { throw new Exception("B2C_1A_LuisRestUserName is not set in configuration"); }
            if (validApiPassword == null) { throw new Exception("B2C_1A_LuisRestUserPassword is not set in configuration"); }

            log.LogTrace($"base64Credentials={base64Credentials}");
            var credentialsBytes = System.Convert.FromBase64String(base64Credentials);
            var credentials = System.Text.Encoding.UTF8.GetString(credentialsBytes).Split(':');
            log.LogTrace($"credentials={credentials}");
            var user = credentials[0];
            var password = credentials[1];
            if (user==validApiUser &&password==validApiPassword)
            {
                log.LogInformation($"Sucessful Basic Authentication");
                return true;
            }
            else
            {
                log.LogWarning($"Basic Authentication Failed");
                return false;
            }

        }
    }
}
