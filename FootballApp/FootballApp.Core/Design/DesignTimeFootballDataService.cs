using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Linq;
//using System.Text;
using FootballApp.Core.Services;
using FootballApp.Core.Model;

namespace FootballApp.Core.Services.DesignTime
{
    public class DesignTimeFootballDataService : IFootballDataService
    {

        public Task<List<Competition>> GetAvailableCompetitionsAsync()
        {
            return Task.Factory.StartNew(() => new List<Competition> 
                                                { 
                                                    new Competition{ Name = "Fake Premier League", Region = "England" },
                                                    new Competition{ Name = "Other Fake League", Region = "Other Region"}
                                                });
        }

        public Task<List<Team>> GetCurrentStandingsAsync(int competitionId)
        {
            return Task.Factory.StartNew(() => new List<Team> 
            { 
                new Team{ TeamName = "Team 1", OverallWins = 10, OverallDraws = 2, OverallLosses = 2, Points = 32, Position = 1, OverallGamesPlayed = 14},
                new Team{ TeamName = "Team 3", OverallWins = 3, OverallDraws = 6, OverallLosses = 5, Points = 15, Position = 3, OverallGamesPlayed = 14},
                new Team{ TeamName = "Team 2", OverallWins = 6, OverallDraws = 6, OverallLosses = 2, Points = 24, Position = 2, OverallGamesPlayed = 14}
            });
        }

        public Task<List<Match>> GetFixturesForDateAsync(int competitionId, DateTime date)
        {
            return Task.Factory.StartNew(() => new List<Match> 
            {
                new Match{ LocalTeamName = "Home 1", VisitorTeamName = "Visitor 1", LocalTeamScore = "?", VisitorTeamScore = "?"},
                new Match{ LocalTeamName = "Home 2", VisitorTeamName = "Visitor 2", LocalTeamScore = "2", VisitorTeamScore = "2"},
            });
        }

        public Task<List<Match>> GetFixturesForDateRangeAsync(int competitionId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
