using Domain.JSONConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UI_CA_Prototype
{
    public class APICalls
    {
        public string API_URL { get; set; }
        // HttpClient http = new HttpClient()
        // HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:POORT/api/course");
        // request.Headers.Add("Accept", "application/json");
        //
        // HttpResponseMessage response = http.SendAsync(request).Result;
        // var responseContentAsString = response.Content.ReadAsStringAsync().Result;
        // requestedData = JsonConvert.DeserializeObject<List<JeObject>>(responseContentAsString);

        public APICalls()
        {
            API_URL = "http://kdg.textgain.com/query";
        }

        public APICalls(string api_url)
        {

        }

        public List<JClass> RequestRecords(string name, DateTime? since = null, DateTime? until = null, Dictionary<string, string[]> themes = null)
        {
            List<JClass> requestedRecords;

            using (HttpClient http = new HttpClient())
            {
                //Request maken
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, API_URL);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");

                //Content in APIQuery steken & in request body zetten
                APIQuery queryObject = new APIQuery();
                queryObject.Name = name;
                if (since != null) queryObject.Since = since;
                if (until != null) queryObject.Until = until;
                if (themes != null) queryObject.Themes = themes;

                string body = JsonConvert.SerializeObject(queryObject);

                //request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                //request.Content = new StringContent("{\"Name\":\"Annick De Ridder\"}", Encoding.UTF8, "application/json");
                request.Content = new StringContent("{\"name\":\"Annick De Ridder\",\"since\":\"" + new DateTime(2018, 1, 1, 0, 0, 0).ToString() + "\",\"until\":\"" + DateTime.Now.ToString() +"\",\"themes\":{ \"religie\":[\"christian\", \"muslim\"],\"media\":[\"nieuws\",\"krant\"]}}", Encoding.UTF8, "application/json");

                //Request naar API zenden
                HttpResponseMessage response = http.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    //Response als string lezen
                    var responseContentAsString = response.Content.ReadAsStringAsync().Result;

                    //String omzetten naar JClass-objecten
                    requestedRecords = JsonConvert.DeserializeObject<List<JClass>>(responseContentAsString);
                }
                else throw new Exception("ERROR: " + response.StatusCode);
            }
            return requestedRecords;
        }
    }
}
