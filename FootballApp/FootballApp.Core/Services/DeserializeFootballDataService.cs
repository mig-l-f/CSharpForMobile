using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FootballApp.Core.Model;

namespace FootballApp.Core.Services
{
    public class DeserializeFootballDataService : IDeserializeFootballDataService
    {
        #region Error Handling
        private bool wasRequestSuccessful(string error)
        {
            if (String.Compare(error, "OK") == 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Data Service Implementation
        public List<Competition> DeserializeCompetitionList(string json)
        {
            JObject rootObject = JObject.Parse(json);

            // Obtain ERROR state
            string errorState = (string)rootObject["ERROR"];
            if (!wasRequestSuccessful(errorState))
                return new List<Competition>();

            IList<JToken> results = rootObject["Competition"].Children().ToList();
            List<Competition> listOfCompetitions = new List<Competition>();
            foreach (JToken result in results)
            {
                Competition competition = JsonConvert.DeserializeObject<Competition>(result.ToString());
                listOfCompetitions.Add(competition);
            }
            return listOfCompetitions;
        }

        public List<Match> DeserializeMatchesList(string json)
        {
            JObject rootObject = JObject.Parse(json);

            // Obtain ERROR state
            string errorState = (string)rootObject["ERROR"];
            if (!wasRequestSuccessful(errorState))
                return new List<Match>();

            IList<JToken> results = rootObject["matches"].Children().ToList();
            List<Match> listOfMatches = new List<Match>();
            foreach (JToken result in results)
            {
                Match match = JsonConvert.DeserializeObject<Match>(result.ToString());
                listOfMatches.Add(match);
            }
            return listOfMatches;
        }

        public List<Team> DeserializeTeamsList(string json)
        {
            JObject rootObject = JObject.Parse(json);

            // Process request error state
            string errorState = (string)rootObject["ERROR"];
            if (!wasRequestSuccessful(errorState))
                return new List<Team>();

            IList<JToken> results = rootObject["teams"].Children().ToList();
            List<Team> listOfTeams = new List<Team>();
            foreach (JToken result in results)
            {
                Team team = JsonConvert.DeserializeObject<Team>(result.ToString());
                listOfTeams.Add(team);
            }
            return listOfTeams;
        }

        #endregion
    }
}
