using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
//using System.Text;
using ToDoList.Core.Model;
using ToDoList.Core.Services;

namespace ToDoList.Core.ViewModel
{
    public class AllToDoItemsViewModel : INotifyPropertyChanged
    {
        public AllToDoItemsViewModel(IToDoItemDataService dataContext) 
        {
            _dataContext = dataContext;
        }

        #region Fields
        private IToDoItemDataService _dataContext;
        private List<ToDoItem> _allToDoItems;
        private List<ToDoItem> _homeToDoItems;
        private List<ToDoItem> _workToDoItems;
        private List<ToDoItem> _hobbiesToDoItems;
        private RelayCommand _deleteToDoItemCommand;
        #endregion

        #region Properties
        public List<ToDoItem> AllToDoItems
        {
            get 
            {
                //if (_allToDoItems == null)
                //{
                _allToDoItems = _dataContext.GetAllToDoItems();
                //}
                return _allToDoItems;
            }
            set
            {
                _allToDoItems = value;
                NotifyPropertyChanged("AllToDoItems");
            }
        }

        public List<ToDoItem> HomeToDoItems
        {
            get
            {
                //if (_homeToDoItems == null)
                //{
                    _homeToDoItems = _dataContext.GetAllToDoItemsForCategory("Home");
                //}
                return _homeToDoItems;
            }

            set
            {
                _homeToDoItems = value;
                NotifyPropertyChanged("HomeToDoItems");
            }
        }

        public List<ToDoItem> WorkToDoItems
        {
            get
            {
                //if (_workToDoItems == null)
                //{
                    _workToDoItems = _dataContext.GetAllToDoItemsForCategory("Work");
                //}
                return _workToDoItems;
            }
            set
            {
                _workToDoItems = value;
                NotifyPropertyChanged("WorkToDoItems");
            }
        } 

        public List<ToDoItem> HobbiesToDoItems
        {
            get
            {
                //if (_hobbiesToDoItems == null)
                //{
                    _hobbiesToDoItems = _dataContext.GetAllToDoItemsForCategory("Hobbies");
                //}
                return _hobbiesToDoItems;
            }
            set
            {
                _hobbiesToDoItems = value;
                NotifyPropertyChanged("HobbiesToDoItems");
            }
        }

        public ICommand DeleteToDoItemCommand
        {
            get
            {
                if (_deleteToDoItemCommand == null)
                {
                    _deleteToDoItemCommand = new RelayCommand(DeleteToDoItem);
                }
                return _deleteToDoItemCommand;
            }
        }
    
        #endregion

        #region INotifyPropertChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Helpers

        private void DeleteToDoItem(object obj)
        {
            ToDoItem deleteToDoItem = obj as ToDoItem;
            if (deleteToDoItem == null)
            {
                return;
            }

            string categoryToBeUpdated = deleteToDoItem.Category.Name;
            _dataContext.DeleteToDoItem(deleteToDoItem);

            NotifyPropertyChanged("AllToDoItems");
            switch (categoryToBeUpdated)
            {
                case "Home":
                    NotifyPropertyChanged("HomeToDoItems");
                    break;
                case "Work":
                    NotifyPropertyChanged("WorkToDoItems");
                    break;
                case "Hobbies":
                    NotifyPropertyChanged("HobbiesToDoItems");
                    break;
            }
        }
        #endregion
    }
}
