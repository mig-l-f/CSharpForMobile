using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.Core.Services;
using ToDoList.Core.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace ToDoList.Core.ViewModel
{
    public class NewToDoItemViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        public NewToDoItemViewModel(IToDoItemDataService dataService)
        {
            _dataService = dataService;
        }

        #region Fields
        private IToDoItemDataService _dataService;
        private List<ToDoCategory> _categoriesList;
        private string _name;
        private string _Error = null;
        private RelayCommand _addNewToDoItemCommand;
        private ToDoCategory _selectedCategory;
        #endregion

        #region Properties
        public List<ToDoCategory> CategoriesList
        {
            get
            {
                _categoriesList = _dataService.GetAllToDoCategories();
                return _categoriesList;
            }
            set { }
        }

        public ICommand AddNewToDoItemCommand
        {
            get
            {
                if (_addNewToDoItemCommand == null)
                {
                    _addNewToDoItemCommand = new RelayCommand(AddNewToDoItem, CanExecuteAddNewToDoItem);
                }
                _addNewToDoItemCommand.UpdateCanExecuteCommand();
                return _addNewToDoItemCommand;
            }
        }

        public string ToDoItemName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("ToDoItemName");
                NotifyPropertyChanged("AddNewToDoItemCommand");
            }
        }

        public ToDoCategory SelectedCategory
        {
            get { return null; }
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged("AddNewToDoItemCommand");
            }
        }

        #endregion

        #region Helpers
        private void AddNewToDoItem(object obj)
        {
            ToDoItem newToDoItem = new ToDoItem 
            {
                ItemName = ToDoItemName,
                Category = _selectedCategory
            };
            _dataService.InsertToDoItem(newToDoItem);
        }

        private bool CanExecuteAddNewToDoItem(object obj)
        {

            if (isPropertyValid("ToDoItemName") & isPropertyValid("SelectedCategory"))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get 
            {
                return _Error;
            }
        }

        public bool isPropertyValid(string propertyName)
        {
            return String.IsNullOrEmpty(this[propertyName]);
        }

        public string this[string propertyName]
        {
            get 
            {
                _Error = string.Empty;
                switch (propertyName)
                {                    
                    case "ToDoItemName":
                        if (String.IsNullOrEmpty(ToDoItemName))
                        {
                            _Error = "Name cannot be empty!";
                        }                        
                        break;
                    case "SelectedCategory":
                        bool containsCategory = 
                            CategoriesList.Any(item => _selectedCategory != null ? item.Name.Equals(_selectedCategory.Name) : false);
                        if (!containsCategory)
                        {
                            _Error = "Category does not exist";
                        }
                        break;
                    default:
                        _Error = "Not evaluated";
                        break;
                }
                return _Error;
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
    }
}
