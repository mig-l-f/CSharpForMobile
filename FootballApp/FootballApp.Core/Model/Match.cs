using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using FootballApp.Core.Helper;

namespace FootballApp.Core.Model
{
    public class Match
    {
        [JsonProperty("match_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int Id { get; set; }

        [JsonProperty("match_static_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int StaticId { get; set; }

        [JsonProperty("match_comp_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int CompId { get; set; }

        [JsonProperty("match_date")]
        public string Date { get; set; }

        [JsonProperty("match_formatted_date")]
        public string FormattedDate { get; set; }

        [JsonProperty("match_season_beta")]
        public string Season { get; set; }

        [JsonProperty("match_week_beta")]
        public string Week { get; set; }

        [JsonProperty("match_venue_beta")]
        public string Venue { get; set; }

        [JsonProperty("match_venue_id_beta")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int VenueId { get; set; }

        [JsonProperty("match_venue_city_beta")]
        public string VenueCity { get; set; }

        [JsonProperty("match_status")]
        public string Status { get; set; }

        [JsonProperty("match_timer")]
        public string Timer { get; set; }

        [JsonProperty("match_time")]
        public string Time { get; set; }

        [JsonProperty("match_commentary_available")]
        public string CommentaryAvailable { get; set; }

        [JsonProperty("match_localteam_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int LocalTeamId { get; set; }

        [JsonProperty("match_localteam_name")]
        public string LocalTeamName { get; set; }

        [JsonProperty("match_localteam_score")]
        public string LocalTeamScore { get; set; }

        [JsonProperty("match_visitorteam_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int VisitorTeamId { get; set; }

        [JsonProperty("match_visitorteam_name")]
        public string VisitorTeamName { get; set; }

        [JsonProperty("match_visitorteam_score")]
        public string VisitorTeamScore { get; set; }

        [JsonProperty("match_ht_score")]
        public string HalfTimeScore { get; set; }

        [JsonProperty("match_ft_score")]
        public string FullTimeScore { get; set; }

        [JsonProperty("match_et_score")]
        public string ExtraTimeScore { get; set; }

        [IgnoreDataMember]
        public List<MatchEvent> match_events { get; set; }
    }
}
