// <copyright file="CompetitionViewModelTest.cs" company="Miguel Fernandes">
//     Miguel Fernandes. All rights reserved.
// </copyright>
// <author>Miguel Fernandes</author>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using FootballApp.Core.Model;
using FootballApp.Core.Services;
using FootballApp.Core.ViewModel;
using FootballApp.Core.ViewModel.Commands;

namespace FootballApp.Test
{
    /// <summary>
    /// Unit tests for the CompetitionViewModel class
    /// </summary>
    [TestFixture, Category("CompetitionViewModelTests")]
    public class CompetitionViewModelTest
    {
        /// <summary>
        /// Tests the command to get current standings is successful and event is raised.
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task TestCommandToGetCurrentStandingsIsSuccessfulAndEventIsRaised()
        {
            List<Team> standings = new List<Team> 
            { 
                new Team { TeamName = "Team1" }, 
                new Team { TeamName = "Team2" } 
            };
            Mock<IFootballDataService> dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.Setup(s => s.GetCurrentStandingsAsync(It.IsAny<int>()))
                .Returns(() => Task.Factory.StartNew(() => 
                { 
                    return standings; 
                }));
            CompetitionViewModel target = new CompetitionViewModel(new Competition { Id = 123 }, dataServiceMock.Object);

            target.GetCurrentStandingsCommand.Execute(null);
            
            while (target.GetCurrentStandingsCommand.Execution.IsNotCompleted) 
            {
            }

            dataServiceMock.Verify(s => s.GetCurrentStandingsAsync(It.IsAny<int>()), Times.Once(), "Get current standing should be called and only once");
            Assert.AreEqual(2, target.GetCurrentStandingsCommand.Execution.Result.Count, "Current Standings should have two elements");
            Assert.AreEqual("Team2", target.GetCurrentStandingsCommand.Execution.Result[1].TeamName, "Name of second team is not the expected");
        }

        /// <summary>
        /// Tests the command to get fixtures for date is successful.
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task TestCommandToGetFixturesForDateIsSuccessful()
        {
            List<FootballApp.Core.Model.Match> listOfMatches = new List<Core.Model.Match> 
            {
                new FootballApp.Core.Model.Match { LocalTeamName = "Home1", VisitorTeamName = "Away1" },
                new FootballApp.Core.Model.Match { LocalTeamName = "Home2", VisitorTeamName = "Away2" },
                new FootballApp.Core.Model.Match { LocalTeamName = "Home3", VisitorTeamName = "Away3" }
            };

            Mock<IFootballDataService> dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.Setup(s => s.GetFixturesForDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(() => Task.Factory.StartNew(() => listOfMatches));

            CompetitionViewModel target = new CompetitionViewModel(new Competition { Id = 123 }, dataServiceMock.Object);

            target.GetTodayFixturesCommand.Execute(null);

            while (target.GetTodayFixturesCommand.Execution.IsNotCompleted) 
            {
            }

            dataServiceMock.Verify(s => s.GetFixturesForDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once(), "Get fixtures should be called and only once");
            Assert.AreEqual(3, target.GetTodayFixturesCommand.Execution.Result.Count, "There should be 3 fixtures today");
            Assert.AreEqual("Away3", target.GetTodayFixturesCommand.Execution.Result.Last().VisitorTeamName, "Visitor for last fixture is not the expected");
        }
    }
}
