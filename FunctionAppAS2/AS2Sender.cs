using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using FunctionAppAS2.Helpers;

namespace FunctionAppAS2
{
    public static class AS2Sender
    {
        [FunctionName("AS2Sender")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();


                byte[] data = Encoding.ASCII.GetBytes(requestBody.ToString());
                ProxySettings proxySettings = new ProxySettings();
                Uri uri = new Uri("http://localhost:7071/api/AS2Receiver");
                AS2Send aS2Send = new AS2Send();

                AS2Send.SendFile(uri, "sendmsg", data, "BZIParty2", "BZIParty1", proxySettings, 20000, "", "", @"BZIParty1.cer");

                return new OkObjectResult("");
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex.Message);
            }
        }
    }
}
