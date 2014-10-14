using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using FootballApp.Core.Helper;

namespace FootballApp.Core.Model
{

    public class Competition
    {        
        [JsonProperty("id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }
}
