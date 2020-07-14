using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ithaca_api
{
    public static class ObtainClaims
    {
        [FunctionName("ObtainClaims")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            /* {
            "objectId": "User objectId",
    "language": "Current UI language"
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

            // Check whether the objectId element is presented
            if (data.objectId == null || data.objectId.ToString() == "")
            {
                return (ActionResult)new BadRequestObjectResult(new ResponseError() { userMessage = "Missing required `objectId` element." });
            }

            // Check whether the language element is presented
            if (data.language == null || data.language.ToString() == "")
            {
                // Set the default value
                data.language = "en-Us";
            }

            Random random = new Random();
            return (ActionResult)new OkObjectResult(new ResponseContent() { balance = $"{random.Next(12345, 99999)}.{random.Next(0, 999)}" });
        }

        public class ResponseContent
        {
            public string balance { get; set; }
        }

        public class ResponseError
        {
            public ResponseError()
            {
                version = "1.0.1";
                status = 409;
            }

            public string version { get; set; }
            public int status { get; set; }
            public string userMessage { get; set; }
        }

    }
}
