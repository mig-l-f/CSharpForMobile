using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ToDoList.Core.Model;
//using System.Text;

namespace ToDoList.Core.ViewModel
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ToDoDataContext toDoDb;
        private ObservableCollection<ToDoItem> _allToDoItems;
        private ObservableCollection<ToDoItem> _homeToDoItems;
        private ObservableCollection<ToDoItem> _workToDoItems;
        private ObservableCollection<ToDoItem> _hobbiesToDoItems;
        private List<ToDoCategory> _categoriesList;
        #endregion

        #region Constructors
        public ToDoViewModel(ToDoDataContext database)
        {
            toDoDb = database;
        }
        #endregion

        #region Properties

        public ObservableCollection<ToDoItem> AllToDoItems
        {
            get
            {
                return _allToDoItems;
            }
            set
            {
                _allToDoItems = value;
                NotifyPropertyChanged("AllToDoItems");
            }
        }

        public ObservableCollection<ToDoItem> HomeToDoItems
        {
            get
            {
                return _homeToDoItems;
            }
            set
            {
                _homeToDoItems = value;
                NotifyPropertyChanged("HomeToDoItems");
            }
        }

        public ObservableCollection<ToDoItem> WorkToDoItems
        {
            get
            {
                return _workToDoItems;
            }
            set
            {
                _workToDoItems = value;
                NotifyPropertyChanged("WorkToDoItems");
            }
        }

        public ObservableCollection<ToDoItem> HobbiesToDoItems
        {
            get 
            {
                return _hobbiesToDoItems;
            }
            set
            {
                _hobbiesToDoItems = value;
                NotifyPropertyChanged("HobbiesToDoItems");
            }
        }

        public List<ToDoCategory> CategoriesList
        {
            get
            {
                return _categoriesList;
            }
            set
            {
                _categoriesList = value;
                NotifyPropertyChanged("CategoriesList");
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Database Helpers

        public void SaveChangesToDb()
        {
            toDoDb.SubmitChanges();
        }

        public void LoadCollectionsFromDatabase()
        {
            var toDoItemsInDb = from ToDoItem todo in toDoDb.Items
                                select todo;

            AllToDoItems = new ObservableCollection<ToDoItem>(toDoItemsInDb);

            var toDoCategoriesInDb = from ToDoCategory category in toDoDb.Categories
                                     select category;

            foreach (ToDoCategory category in toDoCategoriesInDb)
            {
                switch (category.Name)
                {
                    case "Home":
                        HomeToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Work":
                        WorkToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Hobbies":
                        HobbiesToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    default:
                        break;
                }
            }

            CategoriesList = toDoDb.Categories.ToList();

        }

        public void AddToDoItem(ToDoItem newToDoItem)
        {
            toDoDb.Items.InsertOnSubmit(newToDoItem);

            toDoDb.SubmitChanges();

            AllToDoItems.Add(newToDoItem);

            switch (newToDoItem.Category.Name)
            {
                case "Home":
                    HomeToDoItems.Add(newToDoItem);
                    break;
                case "Work":
                    WorkToDoItems.Add(newToDoItem);
                    break;
                case "Hobbies":
                    HobbiesToDoItems.Add(newToDoItem);
                    break;
                default:
                    break;
            }
        }

        public void DeleteToDoItem(ToDoItem deleteToDoItem)
        {
                        
            AllToDoItems.Remove(deleteToDoItem);

            switch (deleteToDoItem.Category.Name)
            {
                case "Home":
                    HomeToDoItems.Remove(deleteToDoItem);
                    break;
                case "Work":
                    WorkToDoItems.Remove(deleteToDoItem);
                    break;
                case "Hobbies":
                    HomeToDoItems.Remove(deleteToDoItem);
                    break;
                default:
                    break;
            }

            toDoDb.Items.DeleteOnSubmit(deleteToDoItem);
            toDoDb.SubmitChanges();
        }
        #endregion
    }
}
