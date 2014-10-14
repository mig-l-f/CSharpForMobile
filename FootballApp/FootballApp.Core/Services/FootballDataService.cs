using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballApp.Core.Model;
using System.Threading.Tasks;

namespace FootballApp.Core.Services
{
    public class FootballDataService : IFootballDataService
    {
        private IDeserializeFootballDataService deserializeFootbalDataService;
        private IDataService dataService;
        private const string apiKey = @"4221fc7b-052a-a555-f4399933af7c";
        private const string apiUrl = @"http://football-api.com/api/";
        private const string parameterSeparator = "&";

        public FootballDataService(IDataService service, IDeserializeFootballDataService deserializeService)
        {
            dataService = service;
            deserializeFootbalDataService = deserializeService;
        }

        #region Request Formatting Helpers
        private string FormatGetCompetitionsRequest()
        {
            return String.Format("{0}?Action={1}&APIKey={2}", apiUrl, "competitions", apiKey);
        }

        private string FormatGetCurrentStandingsRequest(int competitionId)
        {
            return String.Format("{0}?Action={1}&APIKey={2}&comp_id={3}", apiUrl, "standings", apiKey, competitionId);
        }

        private string FormatGetTodaysMatchesRequest(int competitionId)
        {
            return String.Format("{0}?Action={1}&APIKey={2}&comp_id={3}", apiUrl, "today", apiKey, competitionId);
        }

        private string FormatGetFixturesRequest(int competitionId, DateTime matchDate)
        {
            return String.Format("{0}?Action={1}&APIKey={2}&comp_id={3}&&match_date={4}", apiUrl, "fixtures", apiKey, competitionId, matchDate.ToString("dd.MM.yyyy"));
        }

        private string FormatGetFixturesRequest(int competitionId, DateTime fromDate, DateTime toDate)
        {
            return String.Format("{0}?Action={1}&APIKey={2}&comp_id={3}&from_date={4}&to_date={5}",
                apiUrl, "fixtures", apiKey, competitionId, fromDate.ToString("dd.MM.yyyy"), toDate.ToString("dd.MM.yyy"));
        }
        #endregion

        #region Football Data Services

        public async Task<List<Competition>> GetAvailableCompetitionsAsync()
        {
            string url = FormatGetCompetitionsRequest();
            List<Competition> competitions;
            try
            {
                string webResponse = await dataService.MakeJsonWebRequestAsync(url);
                competitions = deserializeFootbalDataService.DeserializeCompetitionList(webResponse);
            }
            catch (Exception)
            {
                competitions = new List<Competition>();
            }
            return competitions;
        }

        public async Task<List<Team>> GetCurrentStandingsAsync(int competitionId)
        {
            string url = FormatGetCurrentStandingsRequest(competitionId);
            List<Team> currentStandings;
            try
            {
                string webResponse = await dataService.MakeJsonWebRequestAsync(url);
                currentStandings = deserializeFootbalDataService.DeserializeTeamsList(webResponse);
            }
            catch (Exception)
            {
                currentStandings = new List<Team>();
            }
            return currentStandings;
        }

        public async Task<List<Match>> GetFixturesForDateAsync(int competitionId, DateTime date)
        {
            string url = FormatGetFixturesRequest(competitionId, date);
            List<Match> fixtures;
            try
            {
                string webResponse = await dataService.MakeJsonWebRequestAsync(url);
                fixtures = deserializeFootbalDataService.DeserializeMatchesList(webResponse);
            }
            catch (Exception)
            {
                fixtures = new List<Match>();
            }
            return fixtures;
        }

        public async Task<List<Match>> GetFixturesForDateRangeAsync(int competitionId, DateTime startDate, DateTime endDate)
        {
            string url = FormatGetFixturesRequest(competitionId, startDate, endDate);
            List<Match> fixtures;
            try
            {
                string webResponse = await dataService.MakeJsonWebRequestAsync(url);
                fixtures = deserializeFootbalDataService.DeserializeMatchesList(webResponse);
            }
            catch (Exception)
            {
                fixtures = new List<Match>();
            }
            return fixtures;
        }

        #endregion
    }
}
