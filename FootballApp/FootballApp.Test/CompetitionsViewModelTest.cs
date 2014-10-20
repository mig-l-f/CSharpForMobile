using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using FootballApp.Core.Services;
using FootballApp.Core.Model;
using FootballApp.Core.ViewModel;

namespace FootballApp.Test
{
    [TestFixture, Category("UnitTests")]
    public class CompetitionsViewModelTest
    {
        Mock<IFootballDataService> dataServiceMock;

        [SetUp]
        public void setUp()
        {
            
        }

        [Test]
        public async Task testCompetitionsListIsCreatedOnStartUp()
        {
            List<Competition> list = new List<Competition> { new Competition { Name = "TestCompetitions" } };
            dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.
                Setup(s => s.GetAvailableCompetitionsAsync()).
                Returns(() => Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(30);
                    return list;
                }));

            CompetitionsViewModel target = new CompetitionsViewModel(dataServiceMock.Object);
            ////target.GetAvailableCompetitionsCommand.ExecuteAsync(null);

            while (target.GetAvailableCompetitionsCommand.Execution.IsNotCompleted) {  }

            dataServiceMock.Verify(mock => mock.GetAvailableCompetitionsAsync(), Times.Once());
            Assert.AreEqual(1, target.GetAvailableCompetitionsCommand.Execution.Result.Count, "Should have returned one competitions");
            Assert.AreEqual("TestCompetitions", target.GetAvailableCompetitionsCommand.Execution.Result.First().Name, "Competition name is not correct");
        }

        [Test]
        public async Task testExceptionOnTaskIsHandledCorrectly()
        {
            dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.
                Setup(s => s.GetAvailableCompetitionsAsync()).
                Returns(() => Task.Factory.StartNew(() =>
                {
                    throw new Exception("This is an error");
                    return new List<Competition>();
                }));

            CompetitionsViewModel target = new CompetitionsViewModel(dataServiceMock.Object);
            while (target.GetAvailableCompetitionsCommand.Execution.IsNotCompleted) { }

            dataServiceMock.Verify(mock => mock.GetAvailableCompetitionsAsync(), Times.Once());
            Assert.IsTrue(target.GetAvailableCompetitionsCommand.Execution.IsFaulted, "Task should have been faulted");
            Assert.AreEqual("This is an error", target.GetAvailableCompetitionsCommand.Execution.ErrorMessage, "Expected message is incorrect");
        }

    }
}
