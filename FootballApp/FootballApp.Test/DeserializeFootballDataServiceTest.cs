using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using FootballApp.Core.Services;
using FootballApp.Core.Model;

namespace FootballApp.Test
{
    [TestFixture, Category("UnitTests")]
    public class DeserializeFootballDataServiceTest
    {
        IDeserializeFootballDataService target;

        [SetUp]
        public void setUp()
        {
            target = new DeserializeFootballDataService();
        }
        
        [Test]
        public void testDeserializeCompetitionData()
        {            
            StreamReader jsonReader = new StreamReader(@"../../Data/competitions.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeCompetitionList(json);
            Assert.AreEqual(1, list.Count, "Competition list should only have one item");
            Assert.AreEqual("Premier League", list[0].Name);
        }

        [Test]
        public void testDeserializedFailedComperitionRequestReturnsEmptyList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/failedRequest.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeCompetitionList(json);
            Assert.IsFalse(list.Any(), "List of competitions should be empty");
        }

        [Test]
        public void testDeserializeFixturesData()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/fixtures.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeMatchesList(json);
            Assert.AreEqual(4, list.Count, "Fixtures list should have 4 matches");
            Assert.AreEqual("Manchester United", list[0].LocalteamName, "Local team for first match is not the expected");
        }

        [Test]
        public void testDeserializeWeekOfFixturesSuccessful()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/fixturesWeekSuccess.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeMatchesList(json);
            Assert.AreEqual(11, list.Count, "Week of Fixtures should have 11 matches");
        }

        [Test]
        public void testDeserializeNoMatchesFoundReturnsEmptyList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/fixturesFailedNoMatches.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeMatchesList(json);
            Assert.IsFalse(list.Any(), "List should be empty");
        }

        [Test]
        public void testDeserializeListOfTeamsIsSuccessful()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/standingsSuccessful.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeTeamsList(json);
            Assert.AreEqual(20, list.Count, "List of teams should have 20 elements");
            Assert.AreEqual("Chelsea", list.First().TeamName, "First place should be Chelsea");
            Assert.AreEqual("Queens Park Rangers", list.Last().TeamName, "Last place should be QPR");
        }

        [Test]
        public void testDeserializeListOfTeamsReturnsEmptyListOnFailedRequest()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/standingsFailedWrongCompetition.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var list = target.DeserializeTeamsList(json);
            Assert.IsFalse(list.Any(), "List of teams should be empty");
        }
    }
}
