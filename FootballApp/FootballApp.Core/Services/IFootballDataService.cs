using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballApp.Core.Model;
using System.Threading.Tasks;

namespace FootballApp.Core.Services
{
    public interface IFootballDataService
    {

        Task<List<Competition>> GetAvailableCompetitionsAsync();
        Task<List<Team>> GetCurrentStandingsAsync(int competitionId);
        //Task<List<Match>> GetTodaysMatchesAsync();
        Task<List<Match>> GetFixturesForDateAsync(int competitionId, DateTime date);
        Task<List<Match>> GetFixturesForDateRangeAsync(int competitionId, DateTime startDate, DateTime endDate);
    }
}
