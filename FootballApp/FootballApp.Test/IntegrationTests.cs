using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FootballApp.Core.Model;
using FootballApp.Core.Services;

namespace FootballApp.Test
{
    [TestFixture, Category("IntegrationTests")]
    public class IntegrationTests
    {
        private FootballDataService target;
        private IDeserializeFootballDataService deserializeService;
        private IDataService dataService;

        [SetUp]
        public void setUp()
        {
            deserializeService = new DeserializeFootballDataService();
            dataService = new DataService();
            target = new FootballDataService(dataService, deserializeService);
        }

        [Test]
        public void testCompetitionRequestToServer()
        {
            List<Competition> competitions = target.GetAvailableCompetitionsAsync().Result;
            Assert.AreEqual(1, competitions.Count, "Number of available competitions is incorrect");
            Assert.AreEqual(1204, competitions.First().Id);
            Assert.AreEqual("Premier League", competitions.First().Name);
        }

        [Test]
        public void testCurrentStandingsRequestToServer()
        {
            List<Team> standings = target.GetCurrentStandingsAsync(1204).Result;
            Assert.AreEqual(20, standings.Count, "Number of teams is not correct");
            Assert.AreEqual("UEFA Champions League", standings.First().PositionDescription, "First place should qualify for Champions League");
            Assert.AreEqual("Relegation", standings.Last().PositionDescription, "Team in last place should be in the relegation zone");
        }
    }
}
