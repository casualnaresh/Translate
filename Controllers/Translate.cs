using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

using System.Windows;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslateController : ControllerBase
    {
        const string COGNITIVE_SERVICES_KEY = "89d0f1ee15404d4a8946d686075f71c4";//"YOUR_COG_SERVICES_SUBSCRIPTION_KEY";
        // Endpoint for Translator Text
        public static readonly string TEXT_TRANSLATION_API_ENDPOINT = "https://api.cognitive.microsofttranslator.com/{0}?api-version=3.0";
        
        [HttpGet]
        public String Get(String text)
        {
            string endpoint = string.Format(TEXT_TRANSLATION_API_ENDPOINT, "translate");
            string uri = string.Format(endpoint + "&from={0}&to={1}", "en", "fr");

            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);
            
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", COGNITIVE_SERVICES_KEY);
                request.Headers.Add("Ocp-Apim-Subscription-Region", "canadacentral");
                request.Headers.Add("X-ClientTraceId", Guid.NewGuid().ToString());

            //     var response =  client.SendAsync(request).Result;
            
            //     var responseBody = response.Content.ReadAsStringAsync();
                
            // return responseBody.Result;
                var response = client.SendAsync(request).Result;
                var responseBody = response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<Dictionary<string, List<Dictionary<string, string>>>>>(responseBody.Result);
                string displayText = "Input Text in English: " + text + "\nTranslated Text in French: " + result[0]["translations"][0]["text"];
                return displayText;

        }
    }
}
