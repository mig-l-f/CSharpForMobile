using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using NUnit.Framework;
using ToDoList.Core.Model;
using ToDoList.Core.Services;
using ToDoList.Core.ViewModel;
using System.Data.Linq;

namespace ToDoList.Test
{
    [TestFixture]
    public class AllToDoViewItemsViewModelTest
    {
        private FakeToDoItemDataService fakeDataContext;

        [SetUp]
        public void setUp()
        {
            fakeDataContext = new FakeToDoItemDataService();
        }

        [Test]
        public void testGettingListOfAllToDos()
        {
            //FakeToDoItemDataService fakeDataContext = new FakeToDoItemDataService();
            AllToDoItemsViewModel target = new AllToDoItemsViewModel(fakeDataContext);

            Assert.AreEqual(4, target.AllToDoItems.Count, "Number of ToDo Items is incorrect");
            Assert.AreEqual("Test1", target.AllToDoItems[0].ItemName, "Name of first item is incorrect");            
        }

        [Test]
        public void testGettingListOfHomeToDos() 
        {
            //FakeToDoItemDataService fakeDataContext = new 
            AllToDoItemsViewModel target = new AllToDoItemsViewModel(fakeDataContext);
            Assert.AreEqual(2, target.HomeToDoItems.Count, "Number of Home ToDo items is incorrect");
            Assert.AreEqual("Test1", target.HomeToDoItems[0].ItemName, "Name of expected item incorrect");
            Assert.AreEqual("Test4", target.HomeToDoItems[1].ItemName, "Name of first item is incorrect");
        }
        [Test]
        public void testGettingListOfWorkToDos()
        {
            AllToDoItemsViewModel target = new AllToDoItemsViewModel(fakeDataContext);
            Assert.AreEqual(1, target.WorkToDoItems.Count, "Number of Home ToDo items is incorrect");
            Assert.AreEqual("Test2", target.WorkToDoItems[0].ItemName, "Name of expected item incorrect");
        }
        [Test]
        public void testGettingListOfHobbiesToDos()
        {
            AllToDoItemsViewModel target = new AllToDoItemsViewModel(fakeDataContext);
            Assert.AreEqual(1, target.HobbiesToDoItems.Count, "Number of Home ToDo items is incorrect");
            Assert.AreEqual("Test3", target.HobbiesToDoItems[0].ItemName, "Name of expected item incorrect");
        }


        [Test]
        public void testPropertyChangeNotificationIsIssued()
        {
            AllToDoItemsViewModel target = new AllToDoItemsViewModel(fakeDataContext);
            bool hasListOfAllToDosChanged = false;
            bool hasListOfHomeToDosChanged = false;
            bool hasListOfWorkToDosChanged = false;
            bool hasListOfHobbiesToDosChanges = false;
            target.PropertyChanged +=
                (obj, args) =>
                {
                    switch (args.PropertyName)
                    {
                        case "AllToDoItems":
                            hasListOfAllToDosChanged = true;
                            break;
                        case "HomeToDoItems":
                            hasListOfHomeToDosChanged = true;
                            break;
                        case "WorkToDoItems":
                            hasListOfWorkToDosChanged = true;
                            break;
                        case "HobbiesToDoItems":
                            hasListOfHobbiesToDosChanges = true;
                            break;
                        default:
                            break;
                    }
                };

            target.AllToDoItems = new List<ToDoItem>() { };
            target.HomeToDoItems = new List<ToDoItem>() { };
            target.WorkToDoItems = new List<ToDoItem>() { };
            target.HobbiesToDoItems = new List<ToDoItem>() { };
            Assert.IsTrue(hasListOfAllToDosChanged, "All To Dos event not fired");
            Assert.IsTrue(hasListOfHomeToDosChanged, "Home To Dos event not fired");
            Assert.IsTrue(hasListOfWorkToDosChanged, "Work To Dos event not fired");
            Assert.IsTrue(hasListOfHobbiesToDosChanges, "Hobbies to dos event not fired");
        }


        [Test]
        public void testDeleteToDoUpdatesListsAccordingly()
        {
            AllToDoItemsViewModel target = new AllToDoItemsViewModel(fakeDataContext);
            bool hasPropertyChangedFired = false;
            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("HomeToDoItems")) hasPropertyChangedFired = true; };
            Assert.AreEqual(4, target.AllToDoItems.Count, "Item count is not correct");
            ToDoItem deleteToDoItem = target.AllToDoItems[3];

            target.DeleteToDoItemCommand.Execute(deleteToDoItem);

            Assert.AreEqual(3, target.AllToDoItems.Count, "All to do items was not updated");
            Assert.AreEqual(1, target.HomeToDoItems.Count, "Home to do items was not updated");
            Assert.IsTrue(hasPropertyChangedFired, "Correct change property was not fired");
        }
    }
}
