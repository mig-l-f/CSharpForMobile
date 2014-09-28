using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;


namespace ToDoList.Core.Model
{
    public class ToDoDataContext : DataContext
    {
        public ToDoDataContext(string connectionString) : base(connectionString) 
        {
        }

        public Table<ToDoItem> Items;

        public Table<ToDoCategory> Categories;
    }
}
