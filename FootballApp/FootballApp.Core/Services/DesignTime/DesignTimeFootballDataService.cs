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
            return Task.Factory.StartNew(() => new List<Competition> { new Competition{ Name = "Premier League" }});
        }

        public Task<List<Team>> GetCurrentStandingsAsync(int competitionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Match>> GetFixturesForDateAsync(int competitionId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<List<Match>> GetFixturesForDateRangeAsync(int competitionId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
