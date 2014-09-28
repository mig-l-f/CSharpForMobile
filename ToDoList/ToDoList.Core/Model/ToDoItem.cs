using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace ToDoList.Core.Model
{
    [Table]
    public class ToDoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Fields
        
        private int _toDoItemId;
        private string _itemName;
        private bool _itemComplete;
        [Column]
        internal int _categoryId;
        private EntityRef<ToDoCategory> _category;
        
        #endregion

        #region Properties

        [Column(IsVersion=true)]
        private Binary _version;

        [Column(IsPrimaryKey=true, IsDbGenerated=true, DbType="INT NOT NULL Identity", CanBeNull=false, AutoSync=AutoSync.OnInsert)]
        public int ToDoItemId 
        {
            get
            {
                return _toDoItemId;
            }
            set 
            {
                if (_toDoItemId != value)
                {
                    NotifyPropertyChanging("ToDoItemId");
                    _toDoItemId = value;
                    NotifyPropertyChanged("ToDoItemId");
                }
            }
        }

        [Column]
        public string ItemName 
        {
            get 
            {
                return _itemName;
            }
            set 
            {
                if (_itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    _itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        [Column]
        public bool ItemComplete
        {
            get 
            {
                return _itemComplete;
            }
            set 
            {
                if (_itemComplete != value)
                {
                    NotifyPropertyChanging("ItemComplete");
                    _itemComplete = value;
                    NotifyPropertyChanged("ItemComplete");
                }
            }
        }

        [Association(Storage="_category", ThisKey="_categoryId", OtherKey="Id", IsForeignKey=true)]
        public ToDoCategory Category
        {
            get 
            {
                return _category.Entity;
            }
            set 
            {
                NotifyPropertyChanging("Category");
                _category.Entity = value;

                if (value != null)
                {
                    _categoryId = value.Id;
                }
                NotifyPropertyChanging("Category");
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

        #region INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion
    }
}
