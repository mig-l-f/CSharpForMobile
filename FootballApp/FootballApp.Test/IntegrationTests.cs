using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NUnit.Framework;
using Moq;
using FootballApp.Core.Model;
using FootballApp.Core.Services;
using FootballApp.Core.ViewModel;
using FootballApp.Core.ViewModel.Commands;
using FootballApp.Core.ViewModel.Services;


namespace FootballApp.Test
{
    [TestFixture, Category("IntegrationTestsWithServerCall")]
    public class IntegrationTestsWithServerCall
    {
        private FootballDataService footballDataService;
        private IDeserializeFootballDataService deserializeService;
        private IDataService dataService;

        [SetUp]
        public void setUp()
        {
            deserializeService = new DeserializeFootballDataService();
            dataService = new DataService();
            footballDataService = new FootballDataService(dataService, deserializeService);
        }

        [Test]
        public async Task testCompetitionRequestToServer()
        {
            List<Competition> competitions = await footballDataService.GetAvailableCompetitionsAsync();
            Assert.AreEqual(1, competitions.Count, "Number of available competitions is incorrect");
            Assert.AreEqual(1204, competitions.First().Id);
            Assert.AreEqual("Premier League", competitions.First().Name);
        }

        [Test]
        public async Task testCurrentStandingsRequestToServer()
        {
            List<Team> standings = await footballDataService.GetCurrentStandingsAsync(1204);
            Assert.AreEqual(20, standings.Count, "Number of teams is not correct");
            Assert.AreEqual("UEFA Champions League", standings.First().PositionDescription, "First place should qualify for Champions League");
            Assert.AreEqual("Relegation", standings.Last().PositionDescription, "Team in last place should be in the relegation zone");
        }

        [Test]
        public async Task testTodayFixturesRequestToServer()
        {
            CompetitionViewModel target = new CompetitionViewModel(new Competition { Id = 1204, Name = "Premier League" }, footballDataService);

            await target.GetTodayFixturesCommand.ExecuteAsync(null);
            
            Assert.IsTrue(target.GetTodayFixturesCommand.Execution.IsCompleted, "Get fixtures should have completed");
            Assert.IsFalse(target.GetTodayFixturesCommand.Execution.IsFaulted, "Get fixtures should not be faulted");
            Assert.IsTrue(target.GetTodayFixturesCommand.Execution.IsSuccessfullyCompleted, "Get fixtures should be successful");
            Console.WriteLine(String.Format("Found {0} fixtures for date {1}", target.GetTodayFixturesCommand.Execution.Result.Count, DateTime.Today.ToString("dd.MM.yyyy")));
        }
        
    }

    [TestFixture, Category("IntegrationTestsWithoutServerCall")]
    public class IntegrationTestsWithoutServerCall
    {
        private SelectCompetitionViewModel target;
        private FootballDataService footballDataService;
        private Mock<IDeserializeFootballDataService> deserializeServiceMock;
        private Mock<IDataService> dataServiceMock;
        private Mock<INavigationService> navigationServiceMock;

        [SetUp]
        public void SetUp()
        {
            deserializeServiceMock = new Mock<IDeserializeFootballDataService>();
            dataServiceMock = new Mock<IDataService>();
            navigationServiceMock = new Mock<INavigationService>();
        }

        [Test]
        public async Task testAsyncCommandReturnsCorrectErrorOnFailedServerCall()
        {
            dataServiceMock.Setup(s => s.MakeJsonWebRequestAsync(It.IsAny<string>()))
                           .Returns(() => Task.Factory.StartNew(() =>
                           {
                               var child = Task.Factory.StartNew(() =>
                               {
                                   throw new HttpRequestException("Http request failure");
                               }, TaskCreationOptions.AttachedToParent);                               
                               return String.Empty;
                           }));
            deserializeServiceMock.Setup(s => s.DeserializeCompetitionList(It.IsAny<string>()))
                                  .Returns(() => 
                                  {
                                      return new List<Competition> { new Competition(){ Name="Failed" }};
                                  });
            footballDataService = new FootballDataService(dataServiceMock.Object, deserializeServiceMock.Object);
            target = new SelectCompetitionViewModel(footballDataService, navigationServiceMock.Object);

            target.GetAvailableCompetitionsCommand.Execute(null);

            while (target.GetAvailableCompetitionsCommand.Execution.IsNotCompleted) { }

            Assert.IsFalse(target.GetAvailableCompetitionsCommand.Execution.IsSuccessfullyCompleted, "Task should not be successful");
            Assert.IsTrue(target.GetAvailableCompetitionsCommand.Execution.IsFaulted, "Task should be failed.");
            Assert.AreEqual("Http request failure", target.GetAvailableCompetitionsCommand.Execution.ErrorMessage, "Wrong error message");
        }

    }


}
