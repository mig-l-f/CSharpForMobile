using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using ToDoList.Core.Services;
using ToDoList.Core.Model;

namespace ToDoList.Test
{
    public class FakeToDoItemDataService : IToDoItemDataService
    {
        public FakeToDoItemDataService()
        {
            _listOfAllItems = new List<ToDoItem>()
                {
                    new ToDoItem {ItemName="Test1", Category= new ToDoCategory { Name = "Home" }, ItemComplete=false},
                    new ToDoItem {ItemName="Test2", Category= new ToDoCategory { Name = "Work" }, ItemComplete=true},
                    new ToDoItem {ItemName="Test3", Category= new ToDoCategory { Name = "Hobbies" }, ItemComplete=false},
                    new ToDoItem {ItemName="Test4", Category= new ToDoCategory { Name = "Home" }, ItemComplete=false}
                };
            _listOfAllCategories = new List<ToDoCategory>() 
                {
                    new ToDoCategory { Name = "Home" },
                    new ToDoCategory { Name = "Hobbies" },
                    new ToDoCategory { Name = "Work" }
                };
        }

        #region Fields
        private List<ToDoItem> _listOfAllItems;
        private List<ToDoCategory> _listOfAllCategories;
        #endregion

        #region Properties

        public List<ToDoItem> ListOfAllItems
        {
            get
            {
                return _listOfAllItems;
            }
        }

        public List<ToDoCategory> ListOfAllCategories
        {
            get
            {
                return _listOfAllCategories;
            }
        }
        #endregion

        #region Methods
        public List<ToDoItem> GetAllToDoItems()
        {
            return ListOfAllItems;
            
        }

        public IEnumerable<ToDoCategory> GetAllToDoCategories()
        {
            throw new NotImplementedException();
        }

        public List<ToDoItem> GetAllToDoItemsForCategory(string categoryName)
        {
            var listOfItemsForCategory =
                from ToDoItem item in ListOfAllItems
                where item.Category.Name.Equals(categoryName)
                select item;
            return listOfItemsForCategory.ToList();

        }

        public void InsertToDoItem(ToDoItem newToDoItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteToDoItem(ToDoItem deleteToDoItem)
        {
            _listOfAllItems.Remove(deleteToDoItem);
        }
        #endregion
    }
}
