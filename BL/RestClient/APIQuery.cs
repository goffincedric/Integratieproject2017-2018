using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PB.BL.RestClient
{
    public class APIQuery
    {
        public APIQuery()
        {
        }

        public APIQuery(string name)
        {
            Name = name;
        }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("since")] public DateTime? Since { get; set; }

        [JsonProperty("until")] public DateTime? Until { get; set; }

        [JsonProperty("themes")] public Dictionary<string, string[]> Themes { get; set; }
    }
}