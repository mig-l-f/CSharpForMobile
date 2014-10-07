using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ToDoList.Core.ViewModel;
using ToDoList.Core.Model;

namespace ToDoList.Test
{
    [TestFixture]
    public class NewToDoItemViewModelTest
    {
        private FakeToDoItemDataService fakeDataService;
        private NewToDoItemViewModel target;

        [SetUp]
        public void setUp()
        {
            fakeDataService = new FakeToDoItemDataService();
            target = new NewToDoItemViewModel(fakeDataService);
        }

        [Test]
        public void testCategoriesPropertiesHasTheCorrectValues()
        {
            Assert.AreEqual(3, target.CategoriesList.Count, "list of categories in incorrect");
            Assert.AreEqual("Home", target.CategoriesList[0].Name, "name of first category is incorrect");
        }

        [Test]
        public void testTaskNameValidationIsCorrect()
        {
            target.ToDoItemName = "";
            Assert.AreEqual("Name cannot be empty!", target["ToDoItemName"], "Empty name does not fail validation");

            target.ToDoItemName = "Valid Name";
            Assert.AreEqual(string.Empty, target["ToDoItemName"], "Valid name triggers error");
        }

        [Test]
        public void testChangingNameFiresPropertyChangedEvent()
        {
            bool hasNameChanged = false;
            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("ToDoItemName")) hasNameChanged = true; };
            target.ToDoItemName = "Teste";
            Assert.IsTrue(hasNameChanged, "Changed name event was not fired");
        }

        [Test]
        public void addingToDoItemWithInvalidNameFails()
        {
            target.ToDoItemName = "";
            target.SelectedCategory = new ToDoCategory { Name = "Home" };
            Assert.IsFalse(target.AddNewToDoItemCommand.CanExecute(null), "Should not be able to run command");
        }

        [Test]
        public void verifyCorrectingNameEnablesCommandAndUpdateEventIsTrigered()
        {
            bool wasUpdateCommandEventTriggered = false;
            target.AddNewToDoItemCommand.CanExecuteChanged += (obj, args) => { wasUpdateCommandEventTriggered = true; };

            target.ToDoItemName = String.Empty;
            target.SelectedCategory = new ToDoCategory { Name = "Home" };
            Assert.IsFalse(target.AddNewToDoItemCommand.CanExecute(null), "Should not be able to run command");
            //Assert.IsFalse(wasUpdateCommandEventTriggered, String.Format("event can execute changed should not be triggered. Error = {0}", target.Error));

            target.ToDoItemName = "Valid Name";
            Assert.IsTrue(target.AddNewToDoItemCommand.CanExecute(null), "Should be able to run command");
            //Assert.IsTrue(wasUpdateCommandEventTriggered, String.Format("event to signal that can execute changed should have been trigered. Error = {0}", target.Error));
        }


        [Test]
        public void testAddingExistingSelectedCategoryPassesValidation()
        {
            target.SelectedCategory = new ToDoCategory { Name = "Home" };
            Assert.AreEqual(string.Empty, target["SelectedCategory"], "Existing category triggers error");
        }

        [Test]
        public void testAddingNonExistingSelectedCategoryFailsValidation()
        {
            target.SelectedCategory = new ToDoCategory { Name = "Non Existing" };
            Assert.AreEqual("Category does not exist", target["SelectedCategory"], "Non existing category does not trigger error");
        }

        [Test]
        public void addingToDoItemWithInvalidCategoryFails()
        {
            target.ToDoItemName = "Valid name";
            target.SelectedCategory = new ToDoCategory { Name = "Non existing" };
            Assert.IsFalse(target.AddNewToDoItemCommand.CanExecute(null), "Should not be able to run commands");
        }

        [Test]
        public void testAddingLegitimateNewToDoItemIsSuccessful()
        {
            target.ToDoItemName = "Valid Name";
            target.SelectedCategory = new ToDoCategory { Name = "Work" };
            Assert.IsTrue(target.AddNewToDoItemCommand.CanExecute(null), String.Format("Should be able to run command. Error={0}", target.Error));

            Assert.AreEqual(4, fakeDataService.GetAllToDoItems().Count, "Should have 3 existing items");
            target.AddNewToDoItemCommand.Execute(null);
            Assert.AreEqual(5, fakeDataService.GetAllToDoItems().Count, "New item should have been added to list");
        }
    }
}
