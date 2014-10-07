using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.Core.Model;

namespace ToDoList.Core.Services
{
    public interface IToDoItemDataService
    {
        List<ToDoItem> GetAllToDoItems();
        List<ToDoCategory> GetAllToDoCategories();
        List<ToDoItem> GetAllToDoItemsForCategory(String categoryName);

        void InsertToDoItem(ToDoItem newToDoItem);
        void DeleteToDoItem(ToDoItem deleteToDoItem);
    }
}
