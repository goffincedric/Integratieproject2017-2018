using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PB.BL.RestClient
{
    public class APIQuery
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("since")]
        public DateTime? Since { get; set; }
        [JsonProperty("until")]
        public DateTime? Until { get; set; }
        [JsonProperty("themes")]
        public Dictionary<string, string[]> Themes { get; set; }

        public APIQuery()
        {

        }

        public APIQuery(string name)
        {
            Name = name;
        }
    }
}
