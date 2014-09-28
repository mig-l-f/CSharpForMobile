using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ToDoList.Core.Model
{
    [Table]
    public class ToDoCategory: INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Fields
        [Column(IsVersion=true)]
        private Binary _version;
        private int _id;
        private string _name;
        private EntitySet<ToDoItem> _todos;
        #endregion

        #region Properties
        [Column(DbType="INT NOT NULL IDENTITY", IsDbGenerated=true, IsPrimaryKey=true)]
        public int Id 
        {
            get 
            {
                return _id;
            }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }
        
        [Column]
        public string Name
        {
            get 
            {
                return _name;
            }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        [Association]
        public EntitySet<ToDoItem> ToDos
        {
            get
            {
                return _todos;
            }
            set
            {
                _todos.Assign(value);
            }
        }
        #endregion

        #region Constructor
        public ToDoCategory()
        {
            _todos = new EntitySet<ToDoItem>(
                new Action<ToDoItem>(attach_ToDo),
                new Action<ToDoItem>(detach_ToDo));
           
        }
        #endregion

        #region Helpers
        private void attach_ToDo(ToDoItem toDo) 
        {
            NotifyPropertyChanging("ToDoItem");
            toDo.Category = this;
        }
        private void detach_ToDo(ToDoItem toDo) 
        {
            NotifyPropertyChanging("ToDoItem");
            toDo.Category = null;
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
