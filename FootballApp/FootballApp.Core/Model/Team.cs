using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using FootballApp.Core.Helper;

namespace FootballApp.Core.Model
{
    public class Team
    {
        [JsonProperty("stand_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int Id { get; set; }

        [JsonProperty("stand_competition_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int CompetitionId { get; set; }

        [JsonProperty("stand_season")]
        public string Season { get; set; }

        [JsonProperty("stand_round")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int Round { get; set; }

        [JsonProperty("stand_stage_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int StageId { get; set; }

        [JsonProperty("stand_group")]
        public string Group { get; set; }

        [JsonProperty("stand_country")]
        public string Country { get; set; }

        [JsonProperty("stand_team_id")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int TeamId { get; set; }

        [JsonProperty("stand_team_name")]
        public string TeamName { get; set; }

        [JsonProperty("stand_status")]
        public string Status { get; set; }

        [JsonProperty("stand_recent_form")]
        public string RecentForm { get; set; }

        [JsonProperty("stand_position")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int Position { get; set; }

        [JsonProperty("stand_overall_gp")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int OverallGamesPlayed { get; set; }

        [JsonProperty("stand_overall_w")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int OverallWins { get; set; }

        [JsonProperty("stand_overall_d")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int OverallDraws { get; set; }

        [JsonProperty("stand_overall_l")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int OverallLosses { get; set; }

        [JsonProperty("stand_overall_gs")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int OverallGoalsScored { get; set; }

        [JsonProperty("stand_overall_ga")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int OverallGoalsAgainst { get; set; }

        [JsonProperty("stand_home_gp")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int HomeGamesPlayed { get; set; }

        [JsonProperty("stand_home_w")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int HomeWins { get; set; }

        [JsonProperty("stand_home_d")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int HomeDraws { get; set; }

        [JsonProperty("stand_home_l")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int HomeLosses { get; set; }

        [JsonProperty("stand_home_gs")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int HomeGoalsScored { get; set; }

        [JsonProperty("stand_home_ga")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int HomeGoalsAgainst { get; set; }

        [JsonProperty("stand_away_gp")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int AwayGamesPlayed { get; set; }

        [JsonProperty("stand_away_w")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int AwayWins { get; set; }

        [JsonProperty("stand_away_d")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int AwayDraws { get; set; }

        [JsonProperty("stand_away_l")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int AwayLosses { get; set; }

        [JsonProperty("stand_away_gs")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int AwayGoalsScored { get; set; }

        [JsonProperty("stand_away_ga")]
        public int AwayGoalsAgainst { get; set; }

        [JsonProperty("stand_gd")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int GoalDifference { get; set; }

        [JsonProperty("stand_points")]
        [JsonConverter(typeof(JsonStringToIntConvertor))]
        public int Points { get; set; }

        [JsonProperty("stand_desc")]
        public string PositionDescription { get; set; }

    }
}
