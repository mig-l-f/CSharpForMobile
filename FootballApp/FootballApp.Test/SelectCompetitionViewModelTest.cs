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
        Mock<IDialogService> dialogServiceMock;

        [SetUp]
        public void setUp()
        {
            dataServiceMock = null;
            navigationMock = null;
            dialogServiceMock = null;
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

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, null, null);
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

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, null, null);
            target.GetAvailableCompetitionsCommand.Execute(null);

            while (target.GetAvailableCompetitionsCommand.Execution.IsNotCompleted) 
            {
            }

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
            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, null, null);

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

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, navigationMock.Object, null);

            target.SelectedCompetition = new Competition() { Name = "TestCompetition", Id = 100 };
            target.NavigateToSelectedCompetitionCommand.Execute(null);

            // Assert IoC container has viewmodel registered
            Assert.IsTrue(SimpleIoc.Default.IsRegistered<CompetitionViewModel>(target.SelectedCompetition.Id.ToString()), "IoC contained should have competitions view model registered as a result of navigating");

            // Assert NavigateTo
            navigationMock.Verify(s => s.NavigateTo(It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async Task TestErrorDialogIsDisplayedWhenExceptionOccurs()
        {
            dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.Setup(s => s.GetAvailableCompetitionsAsync())
                .Returns(() => Task.Factory.StartNew(() => 
                { 
                    throw new Exception("Error Ocurred"); 
                    return new List<Competition>(); 
                }));

            navigationMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, navigationMock.Object, dialogServiceMock.Object);

            target.GetAvailableCompetitionsCommand.Execute(null);

            dialogServiceMock.Verify(s => s.ShowError("Error Ocurred"), Times.Once());
        }

        [Test] 
        public void TestExecutingGetCompetitionCommandTwiceRunsCorrectly()
        {
            Queue<List<Competition>> resultsQueue = new Queue<List<Competition>>(new [] 
            {
                new List<Competition> { new Competition { Id = 1, Name = "List1" } },
                new List<Competition> { new Competition { Id = 2, Name = "List2" } }
            });
            dataServiceMock = new Mock<IFootballDataService>();
            dataServiceMock.Setup(s => s.GetAvailableCompetitionsAsync())
                .Returns(() => Task.Factory.StartNew(() => resultsQueue.Dequeue()));

            navigationMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();

            bool isSuccessfulEventRaised = false;
            bool isCompletedEventRaised = false;

            SelectCompetitionViewModel target = new SelectCompetitionViewModel(dataServiceMock.Object, navigationMock.Object, dialogServiceMock.Object);

            target.GetAvailableCompetitionsCommand.Execution.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
                {
                    isSuccessfulEventRaised = true;
                }
                if (e.PropertyName.Equals("IsCompleted"))
                {
                    isCompletedEventRaised = true;
                }
            };

            target.GetAvailableCompetitionsCommand.Execute(null);

            while (target.GetAvailableCompetitionsCommand.Execution.IsNotCompleted)
            {
            }

            Assert.AreEqual(1, target.GetAvailableCompetitionsCommand.Execution.Result.Count, "1st returned list should have 1 element");
            Assert.AreEqual("List1", target.GetAvailableCompetitionsCommand.Execution.Result[0].Name, "1st returned list is not the expected");
            //Assert.IsTrue(isSuccessfulEventRaised, "1st call to execute should be successful");
            //Assert.IsTrue(isCompletedEventRaised, "1st call to execute should have completed");

            // Reset events for second run
            isSuccessfulEventRaised = false;
            isCompletedEventRaised = false;

            target.GetAvailableCompetitionsCommand.Execute(null);
            while (!target.GetAvailableCompetitionsCommand.Execution.IsCompleted)
            {
            }

            Assert.AreEqual(1, target.GetAvailableCompetitionsCommand.Execution.Result.Count, "2nd returned list should have 1 element");
            Assert.AreEqual("List2", target.GetAvailableCompetitionsCommand.Execution.Result[0].Name, "2nd returned list is not the expected");
            Assert.IsTrue(isSuccessfulEventRaised, "2nd call to execute should be successful");
            Assert.IsTrue(isCompletedEventRaised, "2nd call to execute should have completed");
        }

    }
}
