using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace ithaca_api
{
    public static class CheckLUISRegistration
    {
        
        [FunctionName("check-luis-registration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Database.Logger = log;
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
            if (data.email == null || data.email.ToString() == "")
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = "Missing required `email` element" });
            }

            LuisAccount luisAccount = Database.GetLuisAccount(data.email.ToString());
            if (luisAccount == null)
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = $"email {data.email} address has not been pre-registered with the LUIS service" });
            }

            if (string.IsNullOrEmpty(luisAccount.LuisAccountNumber))
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = $"email {data.email} address has not been pre-registered with the LUIS service" });
            }
            if (!luisAccount.EnableSelfRegistration)
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = $"pre-registration of LUIS account for email {data.email} is currently in progress please try again later" });
            }

            return (ActionResult)new OkObjectResult(luisAccount);
        }

       

      

    }
}
