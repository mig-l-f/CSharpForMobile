using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using ToDoList.Core.Model;

namespace ToDoList.Core.Services
{
    public class ToDoItemDataService : IToDoItemDataService
    {
        private ToDoDataContext _context;

        public ToDoItemDataService(ToDoDataContext context)
        {
            _context = context;
        }

        #region IToDoItemDataService
        
        public List<ToDoItem> GetAllToDoItems()
        {
            return _context.Items.AsEnumerable().ToList();
        }

        public List<ToDoCategory> GetAllToDoCategories()
        {
            return _context.Categories.AsEnumerable().ToList();
        }

        public List<ToDoItem> GetAllToDoItemsForCategory(string categoryName)
        {
            var listOfItemsForCategory =
                from ToDoItem item in _context.Items
                where item.Category.Name.Equals(categoryName)
                select item;
            return listOfItemsForCategory.ToList();
        }

        public void InsertToDoItem(ToDoItem newToDoItem)
        {
            _context.Items.InsertOnSubmit(newToDoItem);
            _context.SubmitChanges();
        }

        public void DeleteToDoItem(ToDoItem deleteToDoItem)
        {
            _context.Items.DeleteOnSubmit(deleteToDoItem);
            _context.SubmitChanges();
        }
        #endregion
    }
}
