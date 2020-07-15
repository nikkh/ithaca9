using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace ithaca_api
{
    public static class GetLUISClaims
    {
        [FunctionName("get-luis-claims")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Database.Logger = log;
            Authenticator.log = log;
            req.Headers.TryGetValue("Authorization", out StringValues authHeaders);
            var authHeader = authHeaders.FirstOrDefault();
            var base64part = authHeader.Split(' ')[1];
            if (!Authenticator.Authenticate(base64part))
            {
                return (ActionResult)new UnauthorizedResult();
            }
            /* {
                "email": "User email address",
             } */

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Check request body
            if (String.IsNullOrEmpty(requestBody))
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = "Request content is empty." });
            }

            // Print out the request body
            log.LogInformation("Request body: " + requestBody);

            // Convert the request body into dynamic JSON data object
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            //Check whether the email element is presented
            if ((data.email == null || data.email.ToString() == "") && (data.signInName == null || data.signInName.ToString() == ""))
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = "either email or signInName must be supplied int he request body" });
            }

            string email = "";
            if (data.email != null) 
            {
                 email = (string) data.email;
            }
            else if (data.signInName != null)
            {
                email = (string) data.signInName;
            }

            LuisAccount luisAccount = Database.GetLuisAccount(email);
            if (luisAccount == null)
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = $"email {email} address has not been pre-registered with the LUIS service" });
            }

            if (string.IsNullOrEmpty(luisAccount.LuisFavoriteColor)) luisAccount.LuisFavoriteColor = "None";
            log.LogDebug($"email={email}, luisFavoritecolor={luisAccount.LuisFavoriteColor}");
            return (ActionResult)new OkObjectResult(new LuisClaimsResponseContent() { LuisFavoriteColor = luisAccount.LuisFavoriteColor, LuisAccountNumber = luisAccount.LuisAccountNumber });
        }

        public class LuisClaimsResponseContent
        {
            public string LuisFavoriteColor { get; set; }
            public string LuisAccountNumber { get; set; }
        }

    }
}
