using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using NUnit.Framework;
using Moq;
using ToDoList.Core.Model;


namespace ToDoList.Test
{
    [TestFixture]
    public class ToDoItemTest
    {
        [Test]
        public void testChangedEventFiresOnToDoItemIdChange()
        {
            ToDoItem target = new ToDoItem();

            bool toDoItemIdChanged = false;

            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("ToDoItemId")) toDoItemIdChanged = true; };

            target.ToDoItemId = 10;
            Assert.IsTrue(toDoItemIdChanged, "Event was not fired on todo item id change");
        }

        [Test]
        public void testChangedEventIsNotFiredWhenToDoItemIdHasTheSameValue()
        {
            ToDoItem target = new ToDoItem();
            target.ToDoItemId = 10;

            bool toDoItemIdChanged = false;
            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("ToDoItemId")) toDoItemIdChanged = true; };
            target.ToDoItemId = 10;

            Assert.IsFalse(toDoItemIdChanged, "Event was fired on todo item id changing to the same value");
        }

        [Test]
        public void testChangedEventFiresTwiceOnLegitimateToDoItemIdChanges()
        {
            ToDoItem target = new ToDoItem();
            bool toDoItemIdChanged = false;
            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("ToDoItemId")) toDoItemIdChanged = true; };
            target.ToDoItemId = 10;
            Assert.IsTrue(toDoItemIdChanged, "Event was not fired on todo item id change");

            toDoItemIdChanged = false;
            target.ToDoItemId = 20;
            Assert.IsTrue(toDoItemIdChanged, "Event was not fired on second todo item id change");
        }

        [Test]
        public void testEventChangingIsFiredOnToDoItemChanging()
        {
            ToDoItem target = new ToDoItem();
            bool isToDoItemIdChanging = false;
            target.PropertyChanging += (obj, args) => { if (args.PropertyName.Equals("ToDoItemId")) isToDoItemIdChanging = true; };
            target.ToDoItemId = 10;
            Assert.IsTrue(isToDoItemIdChanging, "Changing event was not fired on item id change");
        }

        [Test]
        public void testChangedEventFiresCorrectlyOnItemNameChange()
        {
            ToDoItem target = new ToDoItem();
            bool isToDoItemNameChanged = false;
            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("ItemName")) isToDoItemNameChanged = true; };
            target.ItemName = "nome";
            Assert.IsTrue(isToDoItemNameChanged, "Change name event was not fired on first name change");

            isToDoItemNameChanged = false;
            target.ItemName = "nome";
            Assert.IsFalse(isToDoItemNameChanged, "Change name event was fired when name was equal to previous");

            isToDoItemNameChanged = false;
            target.ItemName = "novo nome";
            Assert.IsTrue(isToDoItemNameChanged, "Change event was not fired on second name change");
        }

        [Test]
        public void testChangedEventFiresOnItemCompleteChange()
        {
            ToDoItem target = new ToDoItem();
            bool isToDoItemCompleteChanged = false;
            target.PropertyChanged += (obj, args) => { if (args.PropertyName.Equals("ItemComplete")) isToDoItemCompleteChanged = true; };

            target.ItemComplete = true;
            Assert.IsTrue(isToDoItemCompleteChanged, "Change item complete not fired on first change");

            isToDoItemCompleteChanged = false;
            target.ItemComplete = true;
            Assert.IsFalse(isToDoItemCompleteChanged, "Change item complete was fired when value was the same");

            isToDoItemCompleteChanged = false;
            target.ItemComplete = false;
            Assert.IsTrue(isToDoItemCompleteChanged, "Change item was not fired on second status change");
        }

    }
}
