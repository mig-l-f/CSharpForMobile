using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballApp.Core.Model;

namespace FootballApp.Core.Services
{
    public interface IDeserializeFootballDataService
    {
        List<Competition> DeserializeCompetitionList(string json);

        List<Match> DeserializeMatchesList(string json);

        List<Team> DeserializeTeamsList(string json);
    }
}
