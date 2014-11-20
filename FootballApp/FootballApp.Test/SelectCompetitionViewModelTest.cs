using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using System.Threading;
using FootballApp.Core.Services;
using FootballApp.Core.Model;
using FootballApp.Core.ViewModel;
using FootballApp.Core.ViewModel.Services;

namespace FootballApp.Test
{
    [TestFixture, Category("SelectCompetitionViewModelTests")]
    public class SelectCompetitionViewModelTest
    {
        Mock<IFootballDataService> dataServiceMock;
        Mock<INavigationService> navigationMock;

        [SetUp]
        public void setUp()
        {
            dataServiceMock = null;
            navigationMock = null;
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

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, null);
            target.GetAvailableCompetitionsCommand.Execute(null);

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

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, null);
            target.GetAvailableCompetitionsCommand.Execute(null);

            while (target.GetAvailableCompetitionsCommand.Execution.IsNotCompleted) { }

            dataServiceMock.Verify(mock => mock.GetAvailableCompetitionsAsync(), Times.Once());
            Assert.IsTrue(target.GetAvailableCompetitionsCommand.Execution.IsFaulted, "Task should have been faulted");
            Assert.AreEqual("This is an error", target.GetAvailableCompetitionsCommand.Execution.ErrorMessage, "Expected message is incorrect");
        }

        [Test]
        public void testSelectingCompetitionRaisesProperNotification()
        {
            dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.
                Setup(s => s.GetAvailableCompetitionsAsync()).
                Returns(() => Task.Factory.StartNew(() => new List<Competition>()));
            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, null);

            bool wasSelectedCompetitionChanged = false;
            target.PropertyChanged += (s, e) => { if (e.PropertyName.Equals("SelectedCompetition"))  wasSelectedCompetitionChanged = true; };

            target.SelectedCompetition = new Competition() { Name = "TestCompetition", Id = 120 };
            Assert.AreEqual("TestCompetition", target.SelectedCompetition.Name, "Selected Competition does not have the correct name");
            Assert.IsTrue(wasSelectedCompetitionChanged, "Selected competition change event was not fired");            
        }

        [Test]
        public void testSelectingCompetitionNavigatedToCompetitionsViewModel()
        {
            dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.
                Setup(s => s.GetAvailableCompetitionsAsync()).
                Returns(() => Task.Factory.StartNew(() => new List<Competition>()));

            navigationMock = new Mock<INavigationService>();

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, navigationMock.Object);

            target.SelectedCompetition = new Competition() { Name = "TestCompetition", Id = 100 };
            target.NavigateToSelectedCompetitionCommand.Execute(null);

            // Assert IoC container has viewmodel registered
            Assert.IsTrue(SimpleIoc.Default.IsRegistered<CompetitionViewModel>(target.SelectedCompetition.Id.ToString()), "IoC contained should have competitions view model registered as a result of navigating");

            // Assert NavigateTo
            navigationMock.Verify(s => s.NavigateTo(It.IsAny<Uri>()), Times.Once());
        }
      
    }
}
