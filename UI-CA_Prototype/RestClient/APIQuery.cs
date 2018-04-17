using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.JSONConversion
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
      this.Name = name;
    }
  }
}
