using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using Moq;
using FootballApp.Core.Model;
using FootballApp.Core.Services;
using System.Threading.Tasks;

namespace FootballApp.Test
{
    [TestFixture, Category("UnitTests")]
    public class FootballDataServiceTest
    {
        private FootballDataService target;
        //private Mock<IDeserializeFootballDataService> deserializeMock;
        private DeserializeFootballDataService deserializeService;
        private Mock<IDataService> dataServiceMock;

        [SetUp]
        public void setUp()
        {
            //deserializeMock = new Mock<IDeserializeFootballDataService>();
            deserializeService = new DeserializeFootballDataService();
            dataServiceMock = new Mock<IDataService>();
            target = new FootballDataService(dataServiceMock.Object, deserializeService);
        }

        // Removed Tests because changed visibility of methods to private
        //[Test]
        //public void testRequestStringForAvailableCompetitionsIsCorrect()
        //{              
        //    string expectedRequest = @"http://football-api.com/api/?Action=competitions&APIKey=4221fc7b-052a-a555-f4399933af7c";
        //    string currentRequest = target.FormatGetCompetitionsRequest();
        //    Assert.AreEqual(expectedRequest, currentRequest, "Service request for competetions is not well formatted");
        //}

        //[Test]
        //public void testRequestStringForCurrentStandingsIsCorrect()
        //{
        //    string expectedRequest = @"http://football-api.com/api/?Action=standings&APIKey=4221fc7b-052a-a555-f4399933af7c&comp_id=1204";
        //    string currentRequest = target.FormatGetCurrentStandingsRequest(1204);
        //    Assert.AreEqual(expectedRequest, currentRequest, "Service request for standing is not well formatted");
        //}

        //[Test]
        //public void testRequestStringForTodaysMatchesIsCorrect()
        //{
        //    string expectedRequest = @"http://football-api.com/api/?Action=today&APIKey=4221fc7b-052a-a555-f4399933af7c&comp_id=1204";
        //    string currentRequest = target.FormatGetTodaysMatchesRequest(1204);
        //    Assert.AreEqual(expectedRequest, currentRequest, "Service request for todays matches is not well formatted");
        //}

        //[Test]
        //public void testRequestStringForFixturesWithMatchDateIsCorrect()
        //{
        //    string expectedRequest = @"http://football-api.com/api/?Action=fixtures&APIKey=4221fc7b-052a-a555-f4399933af7c&comp_id=1204&&match_date=12.10.2014";
        //    string currentRequest = target.FormatGetFixturesRequest(1204, new DateTime(2014, 10, 12));
        //    Assert.AreEqual(expectedRequest, currentRequest, "Service request for fixtures is not well formatted");
        //}

        //[Test]
        //public void testRequestStringForFixturesWithDateRangeIsCorrect()
        //{
        //    string expectedRequest = @"http://football-api.com/api/?Action=fixtures&APIKey=4221fc7b-052a-a555-f4399933af7c&comp_id=1204&from_date=29.09.2014&to_date=05.10.2014";
        //    string currentRequest = target.FormatGetFixturesRequest(1204, new DateTime(2014, 09, 29), new DateTime(2014, 10, 05));
        //    Assert.AreEqual(expectedRequest, currentRequest, "Service request for fixtures with date range is not well formatted");
        //}
        // End removed tests
        
        [Test]
        public async void testRequestForAvailableCompetitionsAsyncReturnsCorrectList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/competitions.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>())).Returns(() => Task.Factory.StartNew(() => json));

            List<Competition> competitions = await target.GetAvailableCompetitionsAsync();
            Assert.AreEqual(1, competitions.Count, "Number of available competitions is incorrect");
            Assert.AreEqual(1204, competitions.First().Id);
            Assert.AreEqual("Premier League", competitions.First().Name);
        }


        [Test]
        public async void testRequestForCurrentStandingsAsyncReturnsCorrectList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/standingsSuccessful.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>())).Returns(() => Task.Factory.StartNew(() => json));

            List<Team> standings = await target.GetCurrentStandingsAsync(1204);
            Assert.AreEqual(20, standings.Count, "Number of teams is not correct");
            Assert.AreEqual("Chelsea", standings.First().TeamName, "Name of team in first place is not correct");
            Assert.AreEqual("UEFA Champions League", standings.First().PositionDescription, "First place should qualify for Champions League");
            Assert.AreEqual("Relegation", standings.Last().PositionDescription, "Team in last place should be in the relegation zone");
        }

        [Test]
        public async void testInvalidRequestForCurrentStandingsAsyncReturnsEmptyList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/standingsFailedWrongCompetition.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>())).Returns(() => Task.Factory.StartNew(() => json));

            List<Team> standings = await target.GetCurrentStandingsAsync(1204);
            Assert.IsFalse(standings.Any(), "Failed request should have returned empty list");
        }

        [Test]
        public async void testRequestForFixturesWithMatchDateAsyncReturnCorrectList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/fixtures.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>())).Returns(() => Task.Factory.StartNew(() => json));

            List<FootballApp.Core.Model.Match> fixtures = await target.GetFixturesForDateAsync(1204, new DateTime(2014, 10, 05));
            Assert.AreEqual(4, fixtures.Count, "Number of expected fixtures does not match");
            Assert.AreEqual("Tottenham", fixtures[2].LocalteamName, "Third game local team does not match");
            Assert.AreEqual("Southampton", fixtures[2].VisitorteamName, "Third game visitor team does not match");
        }

        [Test]
        public async void testRequestForFixturesWithDateRangeAsyncReturnsCorrectList()
        {
            StreamReader jsonReader = new StreamReader(@"../../Data/fixturesWeekSuccess.json");
            string json = jsonReader.ReadToEnd();
            jsonReader.Close();
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>())).Returns(() => Task.Factory.StartNew(() => json));

            List<FootballApp.Core.Model.Match> fixtures = await target.GetFixturesForDateRangeAsync(1204, new DateTime(2014, 09, 29), new DateTime(2014, 10, 05));
            Assert.AreEqual(11, fixtures.Count, "Number of expected matches is incorrect");
            Assert.AreEqual("West Ham", fixtures.Last().LocalteamName, "Wrong local team for last match");
            Assert.AreEqual("QPR", fixtures.Last().VisitorteamName, "Wrong visitor team name for last match");
        }

        [Test]
        public async void testRequestForAvailableCompetitionsAsyncOnExceptionThrownAnEmptyListIsReturned()
        {
            string json = "";
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>()))
                           .Returns(() => Task.Factory.StartNew(() => 
                           {
                               var child = Task.Factory.StartNew(() => 
                               {
                                   throw new Exception("Test Exception");
                               });                               
                               return json;
                           }));
            List<Competition> competitions = await target.GetAvailableCompetitionsAsync();
            Assert.IsFalse(competitions.Any(), "Competitions list should be empty");
        }

        [Test]
        public async void testRequestForCurrentStandingsAsyncOnExceptionThrownAnEmptyListIsReturned()
        {
            string json = "";
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>()))
                           .Returns(() => Task.Factory.StartNew(() =>
                           {
                               var child = Task.Factory.StartNew(() =>
                               {
                                   throw new Exception("Test Exception");
                               });
                               return json;
                           }));
            List<Team> standings = await target.GetCurrentStandingsAsync(1204);
            Assert.IsFalse(standings.Any(), "Standings list should be empty");
        }

        [Test]
        public async void testRequestForFixturesForDateAsyncOnExceptionThrownAnEmptyListIsReturned()
        {
            string json = "";
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>()))
                           .Returns(() => Task.Factory.StartNew(() =>
                           {
                               var child = Task.Factory.StartNew(() =>
                               {
                                   throw new Exception("Test Exception");
                               });
                               return json;
                           }));
            List<FootballApp.Core.Model.Match> fixtures = await target.GetFixturesForDateAsync(1204, new DateTime(2014, 10, 05));
            Assert.IsFalse(fixtures.Any(), "Fixtures list should be empty");
        }

        [Test]
        public async void testRequestForFixturesForDateRangeAsyncOnExceptionThrownAnEmptyListIsReturned()
        {
            string json = "";
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>()))
                           .Returns(() => Task.Factory.StartNew(() =>
                           {
                               var child = Task.Factory.StartNew(() =>
                               {
                                   throw new Exception("Test Exception");
                               });
                               return json;
                           }));
            List<FootballApp.Core.Model.Match> fixtures = await target.GetFixturesForDateRangeAsync(1204, new DateTime(2014, 03, 02), new DateTime(2014, 05, 03));
            Assert.IsFalse(fixtures.Any(), "Fixtures list should be empty");
        }
    }
}
